module OutputProcess
// Функция для вывода точек
let printInputPoint (x: float, y: float) (printedPoints: (float * float) list) =
    if not (List.contains (x, y) printedPoints) then
        printfn "< %f;%f" x y
        (x, y) :: printedPoints
    else
        printedPoints

// Функция для вывода интерполированных значений

let printInterpolation (x: float, y: float) (printedPoints: (float * float) list) (method)=
    // Проверяем, была ли уже выведена эта точка
    if not (List.contains (x, y) printedPoints) then
        // Выводим точку и добавляем в список
        if method = "linear" then
            printfn "> linear: %f %f" x y
        else
            printfn "> newton: %f %f" x y

        (x, y) :: printedPoints
    else
        printedPoints