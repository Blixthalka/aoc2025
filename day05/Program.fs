open System
open System.IO

let file = File.ReadAllLines "data.txt"
        |> Seq.filter(fun line -> line <> "")

let ranges = file 
            |> Seq.filter(fun line -> line.Contains('-'))
            |> Seq.map (fun line -> 
                let split = line.Split('-')
                Int128.Parse(split[0]), Int128.Parse(split[1])
            )

let ingredients = file 
                |> Seq.filter(fun line -> not(line.Contains('-')))
                |> Seq.map (fun line -> Int128.Parse line)

let fresh1 = ingredients
                |> Seq.filter (fun i -> ranges |> Seq.exists (fun (low, high ) -> i >= low && i <= high))
                |> Seq.length

Console.WriteLine $"fresh1: {fresh1}"

let fresh2 = 
    ranges 
    |> Seq.toList
    |> List.fold (fun (new_ranges: (Int128 * Int128) list) (cur_low, cur_high) -> 
        let low_inside_opt =  new_ranges |> List.tryFind (fun (r_low, r_high) -> cur_low >= r_low && cur_low <= r_high)
        let high_inside_opt  =  new_ranges |> List.tryFind (fun (r_low, r_high) -> cur_high >= r_low && cur_high <= r_high)

        Console.WriteLine $"cur {new_ranges} {cur_low} {cur_high} asdf2: {low_inside_opt} | {high_inside_opt}"

        let new_low, new_high, toFilter = 
            match low_inside_opt, high_inside_opt with 
                | None, None -> 
                    cur_low, cur_high, new_ranges
                | Some low_inside, Some high_inside -> 
                    let low = [fst low_inside; fst high_inside; cur_low] |> List.min
                    let high = [snd low_inside; snd high_inside; cur_high] |> List.max
                    low, high, new_ranges |> List.filter(fun range -> range <> low_inside && range <> high_inside)
                | Some low_inside, None -> 
                    fst low_inside, cur_high, new_ranges |> List.filter(fun range -> range <> low_inside)
                | None, Some high_inside -> 
                    cur_low, snd high_inside, new_ranges |> List.filter(fun range -> range <> high_inside)

        (new_low, new_high) :: (toFilter |> List.filter (fun (low, high) -> not (low >= new_low && high <= new_high)))
    ) []
    |> List.map (fun (low, high) -> 
        Console.WriteLine $"{low}-{high}"
        high - low + Int128.One
    )
    |> List.sum

Console.WriteLine $"fresh2: {fresh2}"