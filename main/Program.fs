module Program

open System
open Linear
open Lagrange
open InputHandler
open ProcessInput

[<EntryPoint>]
let main argv =
    // Получаем флаги для линейной и лагранжевой интерполяции
    let useLinear = argv |> Array.contains "linear"
    let useLagrange = argv |> Array.contains "lagrange"

    // Получаем значение samplingRate
    let samplingRate =
        argv
        |> Array.tryFind (fun arg -> arg.StartsWith("rate="))
        |> Option.bind (fun rateStr -> 
            match Double.TryParse(rateStr.Substring(5)) with
            | true, rate -> Some rate
            | _ -> None)
        |> Option.defaultValue 1.0

    // Вызов readPoints с правильными логическими значениями и samplingRate
    readPoints useLinear useLagrange samplingRate

    0
