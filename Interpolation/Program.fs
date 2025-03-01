module Program

open InputProcess





[<EntryPoint>]
let main argv =
    try
        let (method, step) = inputData argv
        
        if method = "linear" then
            printfn "Выбран метод линейной интерполяции"
            printfn "Шаг: %f" step
            let points: (float * float) array = [||]
            collectPoints method step points [] // Начинаем собирать точки
            
            
        elif method = "newton" then
            printfn "Выбран метод Ньютона"
            printfn "Шаг: %f" step
            let points: (float * float) array = [||]
            collectPoints method step points [] // Начинаем собирать точки

        else
            printfn "Метод не поддерживается"
        
            
    with
    | ex -> printfn "Ошибка: %s" ex.Message
    
    0