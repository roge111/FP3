module InputProcess

open Linear
open OutputProcess
open System
open Newton


let inputData (argv: string array) = 
    let input: string = argv.[0]
    
    let splitInput = input.Split(";")

 
    let method = splitInput.[0]
    let step = splitInput.[1].Split("=").[1] 

    
    (method, float step)
let appen (array: (float * float) array, point: (float * float)) : (float * float) array = 
    Array.append array [|point|]



// Функция для сбора точек и вывода
// Функция для сбора точек и вывода
let rec collectPoints (method: string) (step: float) (points: (float * float) array) (printedPoints: (float * float) list) =
    let data = Console.ReadLine().Split(";")
    
    if data.Length = 1 && data.[0] = "EOF" then
        // Завершаем работу, выводя последнюю точку
        let lastPoint = Array.last points
        let x = float lastPoint.Item1
        let y = float lastPoint.Item2
        // Выводим последнюю точку
        printInterpolation (x, y) printedPoints
        printfn "Программа завершена."
        raise (System.Exception("Конец ввода. Программа завершена."))
    else
        let x = float data.[0]
        let y = float data.[1]
        
        // Добавляем точку в массив
        let updatedPoints = appen (points, (x, y))
        
        // Обновляем список выведенных точек
        let updatedPrintedPoints = printInputPoint (x, y) printedPoints
        
  

        // Проверяем, есть ли больше двух точек и метод равен "linear"
        if updatedPoints.Length > 2 && method = "linear" then
            let minX = updatedPoints |> Array.minBy fst |> fst
            let maxX = updatedPoints |> Array.maxBy fst |> fst
            
            let rec interpolateInRange currentX printedPoints =
                if currentX > maxX then
                    printedPoints
                else
                    let interpolatedY = linearInterpolation updatedPoints currentX
                    // Печатаем только интерполированные значения
                    let newPrintedPoints = printInterpolation (currentX, interpolatedY) printedPoints method
                    interpolateInRange (currentX + step) newPrintedPoints

            // Интерполяция начинается с шага после минимального X
            let finalPrintedPoints = interpolateInRange (minX + step) updatedPrintedPoints
            // Передаем обновленный список точек в рекурсию
            collectPoints method step updatedPoints finalPrintedPoints
        elif updatedPoints.Length > 4 && method = "newton" then
            let minX = updatedPoints |> Array.minBy fst |> fst
            let maxX = updatedPoints |> Array.maxBy fst |> fst
            
            let rec interpolateInRange currentX printedPoints =
                if currentX > maxX then
                    printedPoints
                else
                    let interpolatedY = newtonInterpolation updatedPoints currentX
                    let newPrintedPoints = printInterpolation (currentX, interpolatedY) printedPoints method
                    interpolateInRange (currentX + step) newPrintedPoints 

            let finalPrintedPoints = interpolateInRange (minX + step) updatedPrintedPoints
            collectPoints method step updatedPoints finalPrintedPoints
    
        else
            // Рекурсивно вызываем collectPoints для продолжения сбора точек
            collectPoints method step updatedPoints updatedPrintedPoints
    

