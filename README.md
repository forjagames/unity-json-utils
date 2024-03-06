# JsonUtils

## Overview

Efficient and lightweight JSON handling tailored to meet diverse requirements. Compatible seamlessly across Unity versions, from 2021 to 2023. Built as a standalone solution, leveraging only C# for optimal compatibility and portability.

## Package contents

### Directories

* `/ForjaGames.Json/Documentation~`: Documentation.
* `/ForjaGames.Json/Runtime`: Source code of the project.
* `/ForjaGames.Json/Samples~`: Example scene.

## Installation instructions

[Installation instructions](https://docs.unity3d.com/Manual/upm-ui-install.html)

**Github URL**
```
https://github.com/forjagames/unity-json-utils.git?path=Assets/JsonUtils
```

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
