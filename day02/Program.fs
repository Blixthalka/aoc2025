open System
open System.IO


let run_test (invalid_fun : int64 -> bool) =
    File.ReadAllLines "data.txt"
        |> Seq.collect (fun line -> line.Split ',' ) 
        |> Seq.choose (fun id -> 
            match id.Split '-' with
                | [| f; s |] -> Some (Int64.Parse f, Int64.Parse s)
                | _ -> None
        )
        |> Seq.collect (fun(f, s) -> seq { f .. s })
        |> Seq.filter (fun number -> invalid_fun number)
        |> Seq.reduce (fun acc value -> acc + value)


let is_invalid_1 (number : int64) = 
    let str = number.ToString()
    if str.Length % 2 = 0 then 
        let first = str[..str.Length / 2 - 1]
        let second = str[str.Length / 2..]
        first = second
    else 
        false

Console.WriteLine("answer1: " + (run_test is_invalid_1).ToString())

let do_check_is_invalid_2 (number_str : string) (split_length : int) = 
    if number_str.Length % split_length = 0 then
        let _, eq = 
            seq {0..number_str.Length / split_length - 1}
                |> Seq.map(fun index -> number_str[index * split_length..(index + 1) * split_length - 1])
                |> Seq.fold (fun (prev, is_equal) number_part ->
                    if prev = "" then
                        number_part, is_equal
                    else 
                        
                        number_part, is_equal && number_part = prev
                ) ("", true)
        eq
    else 
        false

let is_invalid_2 (number : int64)  =
    let number_str = number.ToString()
    seq { 1 .. number_str.Length / 2 }
        |> Seq.map (fun split_length -> do_check_is_invalid_2 number_str split_length)
        |> Seq.exists id



Console.WriteLine("answer1: " + (run_test is_invalid_2).ToString())
