module Linear
let linearInterpolation (points: (float * float) array) (x: float) : float =
    // Сортируем точки по x
    let sortedPoints = Array.sortBy fst points

    // Находим интервал, в котором находится x
    let rec findInterval i =
        if i >= sortedPoints.Length - 1 then
            None
        else if fst sortedPoints.[i] <= x && x <= fst sortedPoints.[i + 1] then
            Some i
        else
            findInterval (i + 1)

    match findInterval 0 with
    | Some i ->
        let (x0, y0) = sortedPoints.[i]
        let (x1, y1) = sortedPoints.[i + 1]
        // Линейная интерполяция
        let slope = (y1 - y0) / (x1 - x0)
        y0 + slope * (x - x0)
    | None -> failwith "x находится вне диапазона известных точек."