# JsonUtils

## Overview

Efficient and lightweight JSON handling tailored to meet diverse requirements. Compatible seamlessly across Unity versions, from 2021 to 2023. Built as a standalone solution, leveraging only C# for optimal compatibility and portability.

## Package contents

### Directories
* `/JsonUtils/Assembly`: Source code of the project.
* `/JsonUtils/Examples`: Example scene.

## Installation instructions

[Installation instructions](https://docs.unity3d.com/Manual/upm-ui-install.html)

## Requirements

* Unity >= 2021.1

## Usage

Encoding:

```
string json = JsonParser.Json(myObject);
```

Decoding:

```
var position = JsonParser.Parse<BallPosition>(json);
```

## Advanced topics

### Diferrences with other Json Parsers

Some differences:
* It can parse `fields` and `properties`.
* The `[Serializable]` attribute is not required. 

Get started with the asset and streamline your JSON handling in Unity effortlessly!

## Need help?

Join our discord! [Discord](https://discord.gg/dqGdSpVcAc)
