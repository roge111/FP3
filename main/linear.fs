module Linear

open System

let linearInterpolate (x1, y1) (x2, y2) (samplingRate: float) (prevPoint: (float * float) option) =
    let (startX, startY) = prevPoint |> Option.defaultValue (x1, y1)

    let rec generatePoints x acc =
        if x > x2 then acc |> List.rev |> (if prevPoint.IsSome then List.tail else id)
        else 
            let t = (x - startX) / (x2 - startX)
            let y = startY + t * (y2 - startY)
            generatePoints (x + samplingRate) ((Math.Round(x, 3), Math.Round(y, 3)) :: acc)

    generatePoints startX []
