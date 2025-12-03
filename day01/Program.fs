open System
open System.IO

let _, password1 = 
    File.ReadAllLines "data.txt"
        |> Seq.map (fun line -> line[0], Int32.Parse(line[1..]))
        |> Seq.fold (fun (pos, sum)  (dir, turn) -> 
            let new_pos = 
                match dir with
                    | 'L' ->  
                        match (pos - turn) % 100 with
                        | x when x < 0 -> 100 + x
                        | x -> x
                    | 'R' -> (pos + turn) % 100
    
            let new_sum = 
                match new_pos with
                    | 0 -> sum + 1
                    | _ -> sum
            new_pos, new_sum
        ) (50, 0)

Console.WriteLine("password1: " + password1.ToString())

let rec do_turn (turns: int) (turn_dir: int) (pos: int) (passed: int) = 
    match turns with 
        | 0 -> 
            pos, passed
        | _ ->
            let new_pos = if pos + turn_dir < 0 then 99 else if pos + turn_dir > 99 then 0 else pos + turn_dir
            let new_passed = passed + if new_pos = 0 then 1 else 0
            do_turn  (turns - 1) turn_dir new_pos new_passed
         

let _, password2 = 
    File.ReadAllLines "data.txt"
        |> Seq.map (fun line -> line[0], Int32.Parse(line[1..]))
        |> Seq.fold (fun (pos, cur_sum)  (dir, turns) -> 
            let turn_dir = if dir = 'L' then -1 else 1
            let new_pos, sum = do_turn turns turn_dir pos 0
            new_pos, cur_sum + sum
        ) (50, 0)

Console.WriteLine("password2: " + password2.ToString())