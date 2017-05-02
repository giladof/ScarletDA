# Project Title

Scarlet Digital Assistant

## Getting Started

At this stage, the Scarlet DA can only run programs.

No special instructions. TBD

### Prerequisites

TBD

## Running the tests

ScarletDA project contains a testing project (ScarletDATest) that is used for testing

### Break down into end to end tests

Code Example
------------
ScarletLib.BaseClasses.Program notepad = new ScarletLib.BaseClasses.Program("Notepad", "notepad.exe", null);
Task<string> result = notepad.RunmeAsync();
while (!result.IsCompleted)
  {
      Console.WriteLine("Current Status: {0}",notepad.ProgramState);
      System.Threading.Thread.Sleep(3000);
  }
Console.WriteLine("Current Status: {0}", notepad.ProgramState);

## Built With
Visual Studio 2015 Community (C#) - https://www.visualstudio.com/thank-you-downloading-visual-studio/?sku=Community&rel=15

## Versioning

Current version is 0.1.0

## Authors

* **Gilad Ofir** - *Senior Application Security Consultant @ AppSec Labs, IL* - [Linkedin](www.linkedin.com/in/gilad-ofir-44959919)

## License

TBD
