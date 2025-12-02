open System
open System.IO

//

let res = 
    File.ReadAllLines("data_1.txt")
        |> Seq.map (fun line -> (line[0], Int32.Parse(line[1..])))
        |> Seq.fold (fun (curr, sum)  (dir, turn) -> 
            let asdf = 
                match dir with
                    | 'L' -> curr - turn
                    | 'R' -> curr + turn
            let asdf2 = 
                match asdf with
                    | x when x < 0 -> 99 + x
                    | x when x > 99 -> x - 99
                    | x -> x
            Console.WriteLine(asdf2)
            let sum2 = 
                match asdf2 with
                    | 0 -> sum + 1
                    | _ -> sum
            (asdf, sum2)
        ) (50, 0)

Console.WriteLine(res)