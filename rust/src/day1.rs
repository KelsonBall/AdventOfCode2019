use std::fs::File;
use std::io::{BufReader, BufRead};

fn fuel_required(mass : i32) -> i32 {
    mass / 3 - 2
}

fn total_fuel_required(mass : i32) -> i32 {     
    match fuel_required(mass) {
        -2 | -1 | 0 => 0,
        other => other + total_fuel_required(other)
    }        
}

pub fn run() {
    println!("--- Day 1: The Tyranny of the Rocket Equation ---");
    let modules : Vec<i32> = 
        BufReader::new(
            File::open("resources/day1input.txt").unwrap())
            .lines()
            .map(|il| il.unwrap().parse().unwrap())
            .collect();

    let mut sum : i32 = 0;
    for module in &modules {
        sum += fuel_required(*module);
    }
    println!("Fuel required for modules: {}", sum);

    sum = 0;
    for module in &modules {
        sum += total_fuel_required(*module);
    }
    println!("Total fuel required: {}", sum);
}

#[cfg(test)]
mod test {
    use super::*;

    macro_rules! fuel_required_tests {
        ($($name:ident: $value:expr,)*) => {
        $(
            #[test]
            fn $name() {
                let (input, expected) = $value;
                assert_eq!(expected, fuel_required(input));
            }
        )*
        }
    }

    fuel_required_tests! { 
        fuel_required_12: (12, 2),
        fuel_required_14: (14, 2),
        fuel_required_1969: (1969, 654),
        fuel_required_100756: (100756, 33583),
    }


    macro_rules! total_fuel_required_tests { ($($name:ident: $value:expr,)*) => { $(
    
    #[test]
    fn $name() {
        let (input, expected) = $value;
        assert_eq!(expected, total_fuel_required(input));
    }

    )*}}

    total_fuel_required_tests! {         
        total_fuel_required_14: (14, 2),
        total_fuel_required_1969: (1969, 966),
        total_fuel_required_100756: (100756, 50346),
    }
}