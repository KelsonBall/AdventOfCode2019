use std::fs::File;
use std::io::{BufRead, BufReader};
use std::collections::VecDeque;

// Written with knowledge of day 5 and 7 puzzles, my first day 2 solution was in csharp 😄

#[derive(Debug, Clone, Copy, PartialEq)]
pub enum ProgramState {
    Running(usize),
    Suspended(usize),
    Error(ErrorState),
    Completed,
}

impl ProgramState {
    fn is_running(&self) -> bool {
        match *self {
            ProgramState::Running(_) => true,
            _ => false,
        }
    }

    fn program_pointer(&self) -> Option<usize> {
        match *self {
            ProgramState::Running(pointer) => Some(pointer),
            ProgramState::Suspended(pointer) => Some(pointer),
            _ => None,
        }
    }
}

#[derive(Debug, Clone, Copy, PartialEq)]
pub enum ErrorState {
    InvalidPointerDereference(i32),
    InvalidOperandCode(i32),
}

#[derive(Debug, Clone, Copy)]
enum Parameter {
    Immediate(i32),
    Position(i32),
}

impl Parameter {
    fn get_from(&self, program: &mut Vec<i32>) -> i32 {
        match *self {
            Parameter::Immediate(value) => value,
            Parameter::Position(pointer) => program[pointer as usize],
        }
    }

    fn get_immediate(&self) -> i32 {
        match *self {
            Parameter::Immediate(value) => value,
            Parameter::Position(pointer) => pointer,
        }
    }

    fn get_pointer(&self, program : &mut Vec<i32>) -> Result<usize, ProgramState> {
        let write_to = self.get_from(program);
        if write_to < 0 || write_to as usize >= program.len() {
             ProgramState::Error(ErrorState::InvalidPointerDereference(write_to));
        }
        Ok(write_to as usize)
    }
}

struct Operation {
    behavior: Box<dyn Fn(&mut Vec<i32>, usize, &Vec<Parameter>, &mut Sys) -> ProgramState>,
    params: Vec<Parameter>,
}

impl Operation {
    fn evaluate(&self, program: &mut Vec<i32>, pointer: usize, system : &mut Sys) -> ProgramState {
        (*self.behavior)(program, pointer, &self.params, system)
    }
}

pub struct Sys {
    program_input: VecDeque<i32>,
    program_output: VecDeque<i32>
}

impl Sys {
    fn read(&mut self) -> Option<i32> {        
        self.program_input.pop_front()        
    }

    fn write(&mut self, value : i32) {
        self.program_output.push_back(value)
    }

    fn recieve(&mut self) -> Option<i32> {        
        self.program_output.pop_front()        
    }

    fn send(&mut self, value : i32) {
        self.program_input.push_back(value)
    }

    fn new() -> Sys {
        Sys {
            program_input : VecDeque::new(),
            program_output: VecDeque::new()
        }
    }
}

fn add_op(program: &mut Vec<i32>, pointer: usize, parameters: &Vec<Parameter>, _: &mut Sys) -> ProgramState {
    match parameters[3].get_pointer(program) {
        Ok(write_ptr) => {
            program[write_ptr] = parameters[1].get_from(program) + parameters[2].get_from(program);
            ProgramState::Running(pointer + parameters.len())
        },
        Err(fail_state) => fail_state
    }    
}

fn mult_op(program: &mut Vec<i32>, pointer: usize, parameters: &Vec<Parameter>, _: &mut Sys) -> ProgramState {
    match parameters[3].get_pointer(program) {
        Ok(write_ptr) => {
            program[write_ptr] = parameters[1].get_from(program) * parameters[2].get_from(program);
            ProgramState::Running(pointer + parameters.len())
        },
        Err(fail_state) => fail_state
    }  
}

fn read_op(program: &mut Vec<i32>, pointer: usize, parameters: &Vec<Parameter>, system: &mut Sys) -> ProgramState {
    match system.read() {
        Some(value) => {
            match parameters[1].get_pointer(program) {
                Ok(write_ptr) => {
                    program[write_ptr] = value;
                    ProgramState::Running(pointer + parameters.len())
                },
                Err(fail_state) => fail_state
            }            
        },
        None => ProgramState::Suspended(pointer)
    }
}

fn write_op(program: &mut Vec<i32>, pointer: usize, parameters: &Vec<Parameter>, system: &mut Sys) -> ProgramState {
    system.write(parameters[1].get_from(program));
    ProgramState::Running(pointer + parameters.len())
}

fn jump_if_true_op(program: &mut Vec<i32>, pointer: usize, parameters: &Vec<Parameter>, _: &mut Sys) -> ProgramState {
    match parameters[1].get_from(program) {
        0 => ProgramState::Running(pointer + parameters.len()),
        _ => match parameters[1].get_pointer(program) {
            Ok(to_ptr) => ProgramState::Running(to_ptr),
            Err(fail_state) => fail_state
        }            
    }
}

fn jump_if_false_op(program: &mut Vec<i32>, pointer: usize, parameters: &Vec<Parameter>, _: &mut Sys) -> ProgramState {
    match parameters[1].get_from(program) {
        0 => match parameters[1].get_pointer(program) {
            Ok(to_ptr) => ProgramState::Running(to_ptr),
            Err(fail_state) => fail_state
        },
        _ => ProgramState::Running(pointer + parameters.len()),    
    }
}

fn lt_op(program: &mut Vec<i32>, pointer: usize, parameters: &Vec<Parameter>, _: &mut Sys) -> ProgramState {
    match parameters[3].get_pointer(program) {
        Ok(write_ptr) => {
            program[write_ptr] = if parameters[1].get_from(program) < parameters[2].get_from(program) { 1 } else { 0 };
            ProgramState::Running(pointer + parameters.len())
        }
        Err(fail_state) => fail_state
    }
}

fn eq_op(program: &mut Vec<i32>, pointer: usize, parameters: &Vec<Parameter>, _: &mut Sys) -> ProgramState {
    match parameters[3].get_pointer(program) {
        Ok(write_ptr) => {
            program[write_ptr] = if parameters[1].get_from(program) == parameters[2].get_from(program) { 1 } else { 0 };
            ProgramState::Running(pointer + parameters.len())
        }
        Err(fail_state) => fail_state
    }
}

fn stop_op(_program: &mut Vec<i32>, _pointer: usize, _parameters: &Vec<Parameter>, _: &mut Sys) -> ProgramState {
    ProgramState::Completed
}

fn invalid_op(_: &mut Vec<i32>, _: usize, parameters: &Vec<Parameter>, _: &mut Sys) -> ProgramState {
    ProgramState::Error(ErrorState::InvalidOperandCode(
        parameters[0].get_immediate(),
    ))
}

fn parameter_from_mode_flags(mode_flag: i32, value: i32) -> Parameter {
    if mode_flag & 1 == 1 {
        Parameter::Immediate(value)
    } else {
        Parameter::Position(value)
    }
}

fn read_next_operation(program: &mut Vec<i32>, pointer: usize) -> Operation {
    let instruction = program[pointer];
    let mode_flags = (instruction / 100) * 100; // clear last 2 decimal digits
    let op_code = instruction - mode_flags;

    match op_code {
        1 => Operation {
            behavior: Box::new(add_op),
            params: vec![
                Parameter::Immediate(1),
                parameter_from_mode_flags(mode_flags, program[pointer + 1]),
                parameter_from_mode_flags(mode_flags / 10, program[pointer + 2]),
                Parameter::Immediate(program[pointer + 3]),
            ],
        },
        2 => Operation {
            behavior: Box::new(mult_op),
            params: vec![
                Parameter::Immediate(2),
                parameter_from_mode_flags(mode_flags, program[pointer + 1]),
                parameter_from_mode_flags(mode_flags / 10, program[pointer + 2]),
                Parameter::Immediate(program[pointer + 3]),
            ],
        },
        99 => Operation {
            behavior: Box::new(stop_op),
            params: vec![Parameter::Immediate(99)],
        },
        other => Operation {
            behavior: Box::new(invalid_op),
            params: vec![Parameter::Immediate(other)],
        },
    }
}

pub fn step(program: &mut Vec<i32>, state: &ProgramState, system: &mut Sys) -> ProgramState {
    match state.program_pointer() {
        Some(ip) => {
            let op = read_next_operation(program, ip);
            op.evaluate(program, ip, system)
        }
        None => state.clone(),
    }
}

pub fn evaluate(program: &mut Vec<i32>, system: &mut Sys) -> ProgramState {
    let mut state = ProgramState::Running(0);
    while state.is_running() {
        state = step(program, &state, system);
    }
    return state;
}

pub fn run() {
    println!("--- Day 5: Sunny with a Chance of Asteroids ---");
    let text = BufReader::new(File::open("resources/day5input.txt").unwrap())
        .lines()
        .next()
        .unwrap()
        .expect("Could not read day2 input line");

    let program: &Vec<i32> = &(text.split(",").map(|i| i.parse().unwrap()).collect());
    panic!("Not implemented");

    let mut system = Sys::new();
    system.send(2);
}

#[cfg(test)]
mod test {
    use super::*;

    #[test]
    fn test_add_and_assign_to_first() {
        let mut program = vec![1, 0, 0, 0, 99];
        let expected = vec![2, 0, 0, 0, 99];
        let mut system = Sys::new();
        let result = evaluate(&mut program, &mut system);
        assert_eq!(result, ProgramState::Completed);
        assert_eq!(program, expected);
    }

    #[test]
    fn test_mult_and_assign_to_empty() {
        let mut program = vec![2, 3, 0, 3, 99];
        let expected = vec![2, 3, 0, 6, 99];
        let mut system = Sys::new();
        let result = evaluate(&mut program, &mut system);
        assert_eq!(result, ProgramState::Completed);
        assert_eq!(program, expected);
    }
}
