use std::fs::File;
use std::io::{BufReader, BufRead};

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
            _ => false
        }
    }

    fn program_pointer(&self) -> Option<usize> {
        match *self {
            ProgramState::Running(pointer) => Some(pointer),
            ProgramState::Suspended(pointer) => Some(pointer),
            _ => None
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
}

struct Operation {
    behavior : Box<dyn Fn(&mut Vec<i32>, usize, &Vec<Parameter>) -> ProgramState>,
    params : Vec<Parameter>
}

impl Operation {
    fn evaluate(&self, program : &mut Vec<i32>, pointer : usize) -> ProgramState {
        (*self.behavior)(program, pointer, &self.params)
    }
}

fn add_op(program : &mut Vec<i32>, pointer : usize, parameters : &Vec<Parameter>) -> ProgramState {
    let write_to = parameters[3].get_immediate();
    if write_to < 0 {
        return ProgramState::Error(ErrorState::InvalidPointerDereference(write_to))
    }

    program[write_to as usize] = parameters[1].get_from(program) + parameters[2].get_from(program);

    ProgramState::Running(pointer + parameters.len())
}

fn mult_op(program : &mut Vec<i32>, pointer : usize, parameters : &Vec<Parameter>) -> ProgramState {
    let write_to = parameters[3].get_immediate();
    if write_to < 0 {
        return ProgramState::Error(ErrorState::InvalidPointerDereference(write_to))
    }

    program[write_to as usize] = parameters[1].get_from(program) * parameters[2].get_from(program);

    ProgramState::Running(pointer + parameters.len())
}

fn stop_op(_program : &mut Vec<i32>, _pointer : usize, _parameters : &Vec<Parameter>) -> ProgramState {
    ProgramState::Completed
}

fn invalid_op(_program : &mut Vec<i32>, _pointer : usize, parameters : &Vec<Parameter>) -> ProgramState {
    ProgramState::Error(ErrorState::InvalidOperandCode(parameters[0].get_immediate()))
}

fn read_next_operation(program : &mut Vec<i32>, pointer : usize) -> Operation {
    match program[pointer] {
        1 => Operation {
                behavior: Box::new(add_op),
                params: vec![
                    Parameter::Immediate(1),
                    Parameter::Position(program[pointer + 1]),
                    Parameter::Position(program[pointer + 2]),
                    Parameter::Immediate(program[pointer + 3])]
            },
        2 => Operation {
                behavior: Box::new(mult_op),
                params: vec![
                    Parameter::Immediate(2),
                    Parameter::Position(program[pointer + 1]),
                    Parameter::Position(program[pointer + 2]),
                    Parameter::Immediate(program[pointer + 3])]
            },
        99 => Operation {
                behavior: Box::new(stop_op),
                params: vec![ Parameter::Immediate(99) ]
            },
        other => Operation {
                behavior: Box::new(invalid_op),
                params: vec![ Parameter::Immediate(other) ]
            }
    }
}

pub fn step(program : &mut Vec<i32>, state : &ProgramState) -> ProgramState {
    match state.program_pointer() {
        Some(ip) => {
            let op = read_next_operation(program, ip);
            op.evaluate(program, ip)
        },
        None => state.clone()
    }
}

pub fn evaluate(program: &mut Vec<i32>) -> ProgramState {
    let mut state = ProgramState::Running(0);
    while state.is_running() {
        state = step(program, &state);
    }
    return state;
}

pub fn run() {
    println!("--- Day 2: 1202 Program Alarm ---");
    let text =
        BufReader::new(
            File::open("resources/day2input.txt").unwrap())
                .lines()
                .next()
                .unwrap()
                .expect("Could not read day2 input line");

    let program : &Vec<i32> = &(text.split(",").map(|i| i.parse().unwrap()).collect());
    println!("Running program with inputs:");
    let mut memory = program.clone();
    memory[1] = 12;
    memory[2] = 2;
    let _ = evaluate(&mut memory);
    println!("Result >> {}", memory[0]);

    let expected = 19690720;
    println!("Finding inputs that evaluate to: {}", expected);
    for noun in 0..99 {
        for verb in 0..99 {
            memory = program.clone();
            memory[1] = noun;
            memory[2] = verb;
            let _ = evaluate(&mut memory);
            if memory[0] == expected {
                println!("Found inputs >> {}, {}", noun, verb);
                return;
            }
        }
    }
}


#[cfg(test)]
mod test {
    use super::*;

    #[test]
    fn test_add_and_assign_to_first() {
        let mut program = vec![ 1, 0, 0, 0, 99 ];
        let expected = vec![ 2, 0, 0, 0, 99 ];
        let result = evaluate(&mut program);
        assert_eq!(result, ProgramState::Completed);
        assert_eq!(program, expected);
    }

    #[test]
    fn test_mult_and_assign_to_empty() {
        let mut program = vec![ 2, 3, 0, 3, 99 ];
        let expected =    vec![ 2, 3, 0, 6, 99 ];
        let result = evaluate(&mut program);
        assert_eq!(result, ProgramState::Completed);
        assert_eq!(program, expected);
    }

}