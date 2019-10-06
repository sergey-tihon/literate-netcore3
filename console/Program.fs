open System
open System.IO
open FSharp.Literate

let docPackagePath path =
    Path.Combine(__SOURCE_DIRECTORY__, @"../packages/docs/", path)
    |> Path.GetFullPath
    |> fun path ->
        if Directory.Exists path then path
        elif File.Exists path then path
        else failwithf "Incorrect path '%s'" path

let includeDir path = "-I:" + docPackagePath path
let reference path  = "-r:" + docPackagePath path
let evaluationOptions =
    [|   includeDir "FSharp.Core/lib/netstandard2.0/"
         includeDir "FSharp.Literate/lib/netstandard2.0/"
         includeDir "FSharp.Compiler.Service/lib/netstandard2.0/"
         reference "FSharp.Compiler.Service/lib/netstandard2.0/FSharp.Compiler.Service.dll" |]
let compilerOptions =
    ("-r:System.Runtime.dlls" :: Array.toList evaluationOptions)
    |> String.concat " "

let parseMd path =
    let doc = Literate.ParseMarkdownFile(path,
                  compilerOptions = compilerOptions,
                  fsiEvaluator = FSharp.Literate.FsiEvaluator(evaluationOptions))
    let body = FSharp.Literate.Literate.FormatLiterateNodes(doc, OutputKind.Html, "", true, true)
    for err in doc.Errors do
        Printf.printfn "%A" err
    body, body.FormattedTips

[<EntryPoint>]
let main argv =
    printfn "\n-----\n%s\n-----\n" compilerOptions
    let file = __SOURCE_DIRECTORY__ + @"/../docs/test.md"
    let (doc, tips) = parseMd file
    let html = Formatting.format doc.MarkdownDocument true OutputKind.Html
    printfn "%O" html
    0
