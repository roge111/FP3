module Tests

open Xunit
open System
open Linear
open Lagrange

[<Fact>]
let ``Linear Interpolation Test`` () =
    let x1, y1 = 1.0, 2.0
    let x2, y2 = 3.0, 4.0
    let samplingRate = 0.5
    let point : (float * float) option = None

    let expected: (float * float) list = [
        (1.0, 2.0); (1.5, 2.5); (2.0, 3.0);
        (2.5, 3.5); (3.0, 4.0)
    ]
    let result: (float * float) list = linearInterpolate (x1, y1) (x2, y2) samplingRate point

    Assert.Equal<(float * float) list>(expected, result)

[<Fact>]
let ``Linear Interpolation Test with negative values`` () =
    let x1, y1 = -3.0, 3.0
    let x2, y2 = -2.0, 2.0
    let samplingRate = 1
    let point : (float * float) option = None

    let expected: (float * float) list = [
        (-3.0, 3.0); (-2.0, 2.0)
    ]
    let result: (float * float) list = linearInterpolate (x1, y1) (x2, y2) samplingRate point

    Assert.Equal<(float * float) list>(expected, result)

[<Fact>]
let ``Linear Interpolation Test with double x value point`` () =
    let x1, y1 = 1.0, 2.0
    let x2, y2 = 3.1, 4.0
    let samplingRate = 0.5
    let point : (float * float) option = None

    let expected: (float * float) list = [
        (1.0, 2.0); (1.5, 2.476); (2.0, 2.952); 
        (2.5, 3.429); (3.0, 3.905); 
    ]
    let result: (float * float) list = linearInterpolate (x1, y1) (x2, y2) samplingRate point

    Assert.Equal<(float * float) list>(expected, result)


[<Fact>]
let ``Lagrange Interpolation Test`` () =
    let points = [
        (1.0, 2.0); (2.0, 3.0);
        (3.0, 5.0); (4.0, 7.0)
    ]
    let samplingRate = 0.5
    let point : (float * float) option = None

    let result: (float * float) list = lagrangeInterpolate points samplingRate point

    let expected = [
        (1.0, 2.0); (1.5, 2.312); (2.0, 3.0); 
        (2.5, 3.938); (3.0, 5.0); (3.5, 6.062);
        (4.0, 7.0)
    ]

    Assert.Equal<(float * float) list>(expected, result)

[<Fact>]
let ``Lagrange Interpolation Test with negative value`` () =
    let points = [
       (-2.0, 2.0); (-1.0, 1.0); 
       (0.0, 0.0); (1.0, -1.0);
    ]
    let samplingRate = 1
    let point : (float * float) option = None

    let result: (float * float) list = lagrangeInterpolate points samplingRate point

    let expected = [
        (-2.0, 2.0); (-1.0, 1.0); 
        (0.0, 0.0); (1.0, -1.0)
    ]

    Assert.Equal<(float * float) list>(expected, result)

[<Fact>]
let ``Lagrange Interpolation Test with double x value point`` () =
    let points = [
        (1.0, 2.0); (2.0, 3.0);
        (3.0, 5.0); (4.5, 7.0)
    ]
    let samplingRate = 1
    let point : (float * float) option = None

    let result: (float * float) list = lagrangeInterpolate points samplingRate point

    let expected = [
        (1.0, 2.0); (2.0, 3.0); (3.0, 5.0); 
        (4.0, 6.686)
    ]

    Assert.Equal<(float * float) list>(expected, result)
