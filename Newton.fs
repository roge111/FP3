module Newton

// Функция интерполяции методом Ньютона
let newtonInterpolation (points: (float * float) array) (x: float) : float =
    let n = points.Length
    let dividedDifferences = Array.init n (fun i -> points.[i].Item2)
    
    for j in 1 .. n - 1 do
        for i in n - 1 .. -1 .. j do
            dividedDifferences.[i] <- (dividedDifferences.[i] - dividedDifferences.[i - 1]) / (points.[i].Item1 - points.[i - j].Item1)
    
    let mutable result = dividedDifferences.[0]
    let mutable product = 1.0
    for i in 1 .. n - 1 do
        product <- product * (x - points.[i - 1].Item1)
        result <- result + dividedDifferences.[i] * product
    result