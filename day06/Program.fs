open System
open System.IO
open System.Text.RegularExpressions;

let file = File.ReadAllLines "data.txt"
        |> Seq.map (fun line -> Regex.Split(line.Trim(), @"\s+" ))
        |> Seq.toArray

let res1 = seq {0 .. file[0].Length - 1} 
        |> Seq.map(fun index -> 
            let operator =
                match file[ file.Length - 1].[index].[0] with
                    | '+' -> (+)
                    | _ -> (*)

            seq {1..file.Length - 2} 
                |> Seq.fold (fun acc i -> operator acc (Int128.Parse(file[i].[index]))) (Int128.Parse(file[0].[index])))
        |> Seq.sum


Console.WriteLine $"res1 {res1}"

let local, total, _ = File.ReadAllLines "data.txt"
                        |> Seq.map (fun line -> line.ToCharArray())
                        |> Seq.toArray
                        |> Array.transpose
                        |> Array.filter (fun line -> not (line |> Array.forall (fun c -> c = ' ')))
                        |> Array.fold (fun (local_sum, total_sum, operator) line -> 
                            if line[line.Length - 1] <> ' ' then 
                                let new_operator = 
                                    match line[line.Length - 1] with
                                        | '+' -> (+)
                                        | _ -> (*)
                                Int128.Parse(line[..line.Length - 2]), total_sum + local_sum, new_operator
                            else
                                operator local_sum (Int128.Parse line), total_sum, operator  
                        ) (Int128.Zero, Int128.Zero, (+))


Console.WriteLine $"res2 {local + total}"