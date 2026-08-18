cwlVersion: v1.2
class: ExpressionTool
label: Gather directories
doc: |
  Helper tool to organize workflow outputs
  
  Takes an array of directories (e.g. from a workflow step) and yields them in a destination directory.

  Adapted from: https://github.com/common-workflow-language/cwl-v1.1/blob/a22b7580c6b50e77c0a181ca59d3828dd5c69143/tests/dir7.cwl

requirements:
  - class: InlineJavascriptRequirement

inputs:
  inDirs: Directory[]
  destinationDir: string

expression: |
  ${
    return {"outDir": {
      "class": "Directory", 
      "basename": inputs.destinationDir,
      "listing": inputs.inDirs
    } };
  }

outputs:
  outDir: Directory