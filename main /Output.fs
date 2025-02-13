module OutputHandler

let printInterpolationResults (results: (float * float) list) title =
    printfn "%s:" title
    
    results |> List.iter (fun (x, y) -> printfn "(%.3f, %.3f)" x y)
    List.tryLast results
