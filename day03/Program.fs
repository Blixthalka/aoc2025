open System
open System.IO

let rec do_find_joltage (bank : int array) (items_left : int) = 
    let max_index, max = 
        bank[..bank.Length - items_left]
            |> Array.mapi (fun i v -> i, v)
            |> Array.maxBy snd

    if items_left = 1 then
        max.ToString()
    else 
        max.ToString() + do_find_joltage bank[max_index + 1 ..] (items_left - 1)


let find_joltage (bank : string) (items : int) = 
    let bank_array =  
        bank.ToCharArray()
            |> Array.map (fun char -> Int32.Parse(char.ToString()))

    let str_joltage = do_find_joltage bank_array items
    
    Int64.Parse str_joltage

let joltage1 = 
    File.ReadAllLines "data.txt"
        |> Seq.map (fun bank ->  find_joltage bank 2)
        |> Seq.reduce (fun a b -> a + b)

Console.WriteLine $"joltage1: {joltage1}"

let joltage2 = 
    File.ReadAllLines "data.txt"
        |> Seq.map (fun bank ->  find_joltage bank 12)
        |> Seq.reduce (fun a b -> a + b)

Console.WriteLine $"joltage2: {joltage2}"
