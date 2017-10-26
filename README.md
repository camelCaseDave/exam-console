A lightweight Q/A stlye <b>exam console</b> to help you revise for your exams.

## In action

![](https://i.imgur.com/OZ9N3dz.gif)

## Usage

1. Add a `.json` file to the root of your project

2. Format the file as follows:

```json
[
    {
        "index": 1,
        "text": "What colour is the sky?",
        "answers": [{
                "index": "a",
                "text": "Red",
                "correct": false
            },
            {
                "index": "b",
                "text": "Blue",
                "correct": true
            },
            {
                "index": "c",
                "text": "Yellow",
                "correct": false
            }
        ]
    },
    {
        "index": 2,
        "text": "Do you prefer Dynamics 365 or Salesforce?",
        "answers": [{
                "index": "a",
                "text": "Dynamics 365",
                "correct": true
            },
            {
                "index": "b",
                "text": "Never heard of it",
                "correct": false
            }
        ]
    }
]
```

3. Build the application as an `.exe` or debug with `F5`

4. Revise and pass all of your exams :heavy_check_mark:
