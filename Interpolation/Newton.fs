module Newton

// Функция интерполяции методом Ньютона без использования mutable
let newtonInterpolation (points: (float * float) array) (x: float) : float =
    let n = points.Length
    
    // Вычисление разделённых разностей
    let dividedDifferences =
        let rec calculateDifferences (differences: float array) j =
            if j = n then differences
            else
                let updatedDifferences = 
                    Array.init n (fun i -> 
                        if i < j then differences.[i]
                        else 
                            (differences.[i] - differences.[i - 1]) / (points.[i].Item1 - points.[i - j].Item1))
                calculateDifferences updatedDifferences (j + 1)
        calculateDifferences (Array.map snd points) 1
    
    // Вычисление значения интерполяции
    let rec computeResult i product result =
        if i = n then result
        else
            let newProduct = product * (x - points.[i - 1].Item1)
            computeResult (i + 1) newProduct (result + dividedDifferences.[i] * newProduct)
    
    computeResult 1 1.0 dividedDifferences.[0]
