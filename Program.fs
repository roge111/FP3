module InputHandler

open System

let processPoints points =
    printfn "Обработано %d точек" (List.length points)
    (None, None)  // Заглушка для возвращаемого значения

let readPoints useLinear useLagrange samplingRate =
    printfn "Введите координаты точки (x y) или 'exit' для завершения."

    let rec loop points =
        printf ">> "
        match Console.ReadLine() with
        | null | "exit" -> printfn "Выход."; ()
        | input when String.IsNullOrWhiteSpace(input) ->
            printfn "Ошибка: пустой ввод. Попробуйте снова."
            loop points
        | input ->
            let values = input.Split() |> Array.map Double.TryParse
            match values with
            | [| (true, x); (true, y) |] ->
                let updatedPoints = 
                    (x, y) :: points 
                    |> List.truncate 4

                printfn "Добавлена точка (%.3f, %.3f)." x y

                processPoints updatedPoints |> ignore
                loop updatedPoints
            | _ ->
                printfn "Ошибка: Введите два числа через пробел."
                loop points

    loop []

// Вызов функции (если нужно протестировать)
readPoints true true 10
