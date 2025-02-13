module Lagrange

open System

let lagrangeInterpolate (points: (float * float) list) (samplingRate: float) (startPoint: (float * float) option) =
    let rec interpolate x acc =
        match points with
        | [] -> List.rev acc
        | (minX, _) :: _ ->
            let maxX = points |> List.last |> fst
            if x > maxX then List.rev acc
            else
                let y =
                    points
                    |> List.mapi (fun i (xi, yi) ->
                        let li =
                            points
                            |> List.mapi (fun j (xj, _) -> if i <> j then (x - xj) / (xi - xj) else 1.0)
                            |> List.fold (*) 1.0
                        yi * li)
                    |> List.sum
                interpolate (x + samplingRate) ((Math.Round(x, 3), Math.Round(y, 3)) :: acc)

    let startX = startPoint |> Option.map fst |> Option.defaultValue (fst (List.head points))
    
    interpolate startX [] |> (if startPoint.IsSome then List.tail else id)
