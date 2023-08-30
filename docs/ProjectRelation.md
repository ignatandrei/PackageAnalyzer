
# Project relations

```mermaid
---
title: Project Relations
---
flowchart TB    

NetTilt.Context-->NetTilt.Models

NetTilt.WebAPI-->NetTilt.Auth

NetTilt.WebAPI-->NetTilt.Logic

NetTilt.WebAPI-->NetTilt.Context

NetTilt.Auth-->NetTilt.Context

NetTilt.Tests-->NetTilt.Logic

NetTilt.Logic-->NetTilt.Auth

NetTilt.Logic-->NetTilt.Context
```
<small>Generated  by https://www.nuget.org/packages/netpackageanalyzerconsole , version 7.2023.830.921</small>

