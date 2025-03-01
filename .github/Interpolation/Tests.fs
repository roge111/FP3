module Tests

open Linear
open Newton
open NUnit.Framework

[<Test>]
let ``Linear interpolation multiple points test`` () =
    let points = [| (0.0, 0.0); (2.0, 2.0); (4.0, 4.0) |]
    let step = 1.0

    let expectedPoints = 
        [ (0.0, 0.0); (1.0, 1.0); (2.0, 2.0); (3.0, 3.0); (4.0, 4.0) ]

    let mutable resultPoints = []
    let rec collectLinearPoints currentX =
        if currentX <= 4.0 then
            let y = linearInterpolation points currentX
            resultPoints <- resultPoints @ [(currentX, y)]
            collectLinearPoints (currentX + step)
    collectLinearPoints 0.0

    Assert.Equals(expectedPoints, resultPoints)


[<Test>]
let ``Newton interpolation points test`` () =
    let points = [| (0.0, 0.0); (1.0, 1.0); (2.0, 2.0); (3.0, 3.0) |]
    let step = 0.5

    let expectedPoints = 
        [ (0.0, 0.0); (0.5, 0.5); (1.0, 1.0); (1.5, 1.5); 
          (2.0, 2.0); (2.5, 2.5); (3.0, 3.0) ]

    let mutable resultPoints = []
    let rec collectNewtonPoints currentX =
        if currentX <= 3.0 then
            let y = newtonInterpolation points currentX
            resultPoints <- resultPoints @ [(currentX, y)]
            collectNewtonPoints (currentX + step)
    collectNewtonPoints 0.0

    Assert.Equals(expectedPoints, resultPoints)
