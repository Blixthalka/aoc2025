open System
open System.IO
open System.Collections.Generic

let file = File.ReadAllLines "data.txt"
        |> Seq.map (fun s -> s.ToCharArray())
        |> Seq.toArray

let start_x_coord = file[0]
                    |> Array.mapi (fun i c -> i, c) 
                    |> Array.find (fun (i,  c) -> c = 'S')
                    |> fst

let rec move_tachyon_beams positions splits_in = 
    let s, p = positions 
            |> Array.fold (fun (splits, new_pos)(y, x) -> 
                    if y + 1 >= file.Length then 
                        splits, new_pos
                    else 
                        match file[y + 1][x] with 
                            | '^' -> splits + 1, Array.append new_pos[|y + 1, x - 1; y + 1, x + 1|]
                            |  _  -> splits, Array.append new_pos [|y + 1, x|]
                ) (splits_in, [||])
    if p.Length = 0 then 
        s
    else 
        move_tachyon_beams (p |> Array.distinct) s

let res1 = move_tachyon_beams [|(0, start_x_coord)|] 0
Console.WriteLine $"res1 {res1}"

let lookup : Dictionary<(int * int), Int128> = Dictionary []

let rec move_tachyon_beam (y, x) =
    if lookup.ContainsKey(y, x) then 
        lookup[(y, x)]
    else 
        if y + 1 >= file.Length then 
            Int128.One
        else 
            let v = 
                match file[y + 1][x] with 
                | '^' -> move_tachyon_beam (y + 1, x - 1) + move_tachyon_beam (y + 1, x + 1)
                |  _  -> move_tachyon_beam (y + 1, x)
            lookup.Add((y,x), v)
            v


let res2 = move_tachyon_beam (0, start_x_coord)
Console.WriteLine $"res2 {res2}"