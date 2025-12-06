open System
open System.IO



let file = File.ReadAllLines "data.txt"
        |> Array.map (fun line -> line.ToCharArray())

let create_coord_seq (y_min: int) (y_max: int) (x_min : int) (x_max: int) = 
    seq {y_min .. y_max}
            |> Seq.collect (fun y -> seq {x_min..x_max}
                                    |> Seq.map (fun x -> y, x))

let print_file () =
    file |> Array.iter (fun row -> Console.WriteLine(System.String(row)))

let count_neighbours (y : int) (x: int) = 
    create_coord_seq (y - 1) (y + 1) (x - 1) (x + 1)
        |> Seq.filter (fun (cy, cx) -> not (cy = y && cx = x))
        |> Seq.filter (fun (cy, cx) -> cy >= 0 && cx >= 0 && cy < file.Length && cx < file[0].Length)
        |> Seq.filter (fun (cy, cx) -> file[cy][cx] = '@')
        |> Seq.length

let removed_1 = create_coord_seq 0 (file.Length - 1) 0 (file[0].Length - 1)
                |> Seq.filter (fun (y, x) -> file[y][x] = '@')
                |> Seq.filter (fun (y, x) -> count_neighbours y x < 4)
                |> Seq.length


Console.WriteLine $"part1: {removed_1}"

let rec do_remove () = 
    let removed_items = create_coord_seq 0 (file.Length - 1) 0 (file[0].Length - 1)
                        |> Seq.filter (fun (y, x) -> file[y][x] = '@')
                        |> Seq.filter (fun (y, x) -> count_neighbours y x < 4)
                        |> Seq.toArray

    let removed = removed_items |> Array.length
    removed_items |> Array.iter (fun (y, x) -> file[y][x] <- 'x')

    if removed = 0 then 
        removed
    else 
        removed + do_remove() 


let removed_2 = do_remove()


Console.WriteLine $"part2: {removed_2}"
