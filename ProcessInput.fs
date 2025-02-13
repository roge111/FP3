module ProcessInput

open Linear
open Lagrange
open Output

let processPoints (points: (float * float) list) (startPoints: (float * float) option * (float * float) option) useLinear useLagrange samplingRate =
    let interpolateLinear () =
        match points |> List.rev with
        | (x2, y2) :: (x1, y1) :: _ when useLinear ->
            let result = linearInterpolate (x1, y1) (x2, y2) samplingRate (fst startPoints)
            printInterpolationResults result "Результат линейной интерполяции"
            Some result
        | _ -> None

    let interpolateLagrange () =
        if List.length points >= 4 && useLagrange then
            let result = lagrangeInterpolate points samplingRate (snd startPoints)
            printInterpolationResults result "Результат интерполяции Лагранжем"
            Some result
        else None

    (interpolateLinear (), interpolateLagrange ())
