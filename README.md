# Лабораторная работа 3
Выаолнил: Батаргин Егор Александрович

Группа: P3332

ITMO.ID: 335189
#

В рамках лабораторной работы вам предлагается повторно реализовать лабораторную работу по предмету "Вычислительная математика" посвящённую интерполяции (в разные годы это лабораторная работа 3 или 4) со следующими дополнениями:

обязательно должна быть реализована линейная интерполяция (отрезками, link);
настройки алгоритма интерполяции и выводимых данных должны задаваться через аргументы командной строки:

какие алгоритмы использовать (в том числе два сразу);
частота дискретизации результирующих данных;
и т.п.;


входные данные должны задаваться в текстовом формате на подобии ".csv" (к примеру x;y\n или x\ty\n) и подаваться на стандартный ввод, входные данные должны быть отсортированы по возрастанию x;
выходные данные должны подаваться на стандартный вывод;
программа должна работать в потоковом режиме (пример -- cat | grep 11), это значит, что при запуске программы она должна ожидать получения данных на стандартный ввод, и, по мере получения достаточного количества данных, должна выводить рассчитанные точки в стандартный вывод;

# **InputProcess**
InputProcess - основной файл для считывания данных, вводимы с клавиатуры и обработки точек.

Вот полный код

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
    

Функция inputData - отвечает за ввод данных с командой строки в виде dotnet run newton;step=0.5 С помщью Split мы разбиваем на входную строку на список и забираем данные. Для шага мы разбиваем данные по знаку разделителю "=", чтобы забрать значение шага.

appen - организует добавление элементов в список. Добавление идет рекусривно, поэтому соблюдается принцип функционлаьного программирования. Так же у нас есть условие, что если точек больше двух, то мы начинаем выводить результат линейной интерполяции, если того попросил пользователь или если точек от 4-х и больше и метод выбран newton, то мы выводим результат интерполяции Ньютона. Однако, чтбы не захламлять вывод, мы делаем вывод точек интерполяции, кторые еще не выводили. Такое обеспечивается внтури_** printInterpolation**_ .

**Linear. Newton**

В этих файлах лежат методы, реализующие интерполяции соотвествующие.

Метод линейной интерполяции:

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


  И метод Ньютона

    module Newton

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

**OutputProcess**

Здесь организован вывод точек, при этом здесь именно идет проверка того, что точки еще не выведены 


    module OutputProcess
    let printInputPoint (x: float, y: float) (printedPoints: (float * float) list) =
        if not (List.contains (x, y) printedPoints) then
            printfn "< %f;%f" x y
            (x, y) :: printedPoints
        else
            printedPoints
    
    
    let printInterpolation (x: float, y: float) (printedPoints: (float * float) list) (method)=
        if not (List.contains (x, y) printedPoints) then
            if method = "linear" then
                printfn "> linear: %f %f" x y
            else
                printfn "> newton: %f %f" x y
    
            (x, y) :: printedPoints
        else
        printedPoints


**Примеры**
Пример с линейной интерполяцией 

    < dotnet run linear;step=0.5
    
    > Выбран метод линейной интерполяции
    > Шаг: 0.500000
    < 0;0
    > 0.000000;0.000000
    1;1
    > 1.000000;1.000000
    < 2;2
    > 2.000000;2.000000
    > linear: 0.500000 0.500000
    > linear: 1.500000 1.500000
    < 3;3
    > 3.000000;3.000000
    > linear: 2.500000 2.500000
    < 4;4
    > 4.000000;4.000000
    > linear: 3.500000 3.500000
    < 5;5
    > 5.000000;5.000000
    > linear: 4.500000 4.500000
    <EOF
    >Программа завершена.
    >Ошибка: Конец ввода. Программа завершена.

Пример с методом Ньютона

    < dotnet run newton;step=0.5
    > Выбран метод Ньютона
    > Шаг: 0.500000
    < 1;1
    > 1.000000;1.000000
    < 2;2
    > 2.000000;2.000000
    < 3;3
    > 3.000000;3.000000
    < 7;7
    > 7.000000;7.000000
    < 8;8
    > 8.000000;8.000000
    > newton: 1.500000 1.500000
    > newton: 2.500000 2.500000
    > newton: 3.500000 3.500000
    > newton: 4.000000 4.000000
    > newton: 4.500000 4.500000
    > newton: 5.000000 5.000000
    > newton: 5.500000 5.500000
    > newton: 6.000000 6.000000
    > newton: 6.500000 6.500000
    > newton: 7.500000 7.500000
    <EOF
    >Программа завершена.
    >Ошибка: Конец ввода. Программа завершена.




