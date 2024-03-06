# UnityJsonUtils

Efficient and lightweight JSON handling tailored to meet diverse requirements. Compatible seamlessly across Unity versions, from 2021 to 2023. Built as a standalone solution, leveraging only C# for optimal compatibility and portability.

## Code
### Directories
* `/JsonUtils/Assembly`: Source code of the project.
* `/JsonUtils/Examples`: Example scene.

## Usage

Encoding:

```
string json = JsonParser.Json(myObject);
```

Decoding:

```
var position = JsonParser.Parse<BallPosition>(json);
```

## Diferrences with other Json Parsers

Some differences:
* It can parse `fields` and `properties`.
* The `[Serializable]` attribute is not required. 

Get started with the asset and streamline your JSON handling in Unity effortlessly!

## Need help?

Join our discord! [Discord](https://discord.gg/dqGdSpVcAc)
