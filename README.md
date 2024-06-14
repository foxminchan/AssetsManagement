# Rookie-Phase 2: Assets Management

## Introduction

This is the second phase of the Rookie project. In this phase, we will focus on the assets management of the project.

## Requirements

- [.NET 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/en/)
- [Azure DevOps Account](https://dev.azure.com/)

## Getting Started

1. Clone the repository: [git clone](https://rookies2021group2team1@dev.azure.com/rookies2021group2team1/2024Batch7Net3/_git/2024Batch7Net3)
2. Navigate to the project directory: `cd 2024Batch7Net3`
3. Install the required tools

```bash
npm install
dotnet restore
dotnet tool restore
cd src/ASM.Web && npm install
```

4. Run the project

```bash
dotnet run --project src/ASM.Api/ASM.Api.csproj
cd src/ASM.Web && npm run dev
```

## Developer Guide

### Check Warnings and Errors

For BE project, use the following command to check warnings and errors:

```bash
dotnet build
```

For FE project, use the following command to check warnings and errors:

```bash
npm run lint
```

### Format code before commit

For BE project, use the following command to format the code:

```bash
dotnet format
```

For FE project, use the following command to format the code:

```bash
npm run format:write
```

### Commit message convention

(Ticket ID) (Ticket Title)

Example:

`Commit 92327ba3: (3593) User is able to log into the system, so that user can access the system according to user’s authority`

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
