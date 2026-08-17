#!/usr/bin/env cwl-runner
cwlVersion: v1.2
class: Workflow

requirements:
  - class: ScatterFeatureRequirement
  - class: StepInputExpressionRequirement
  - class: InlineJavascriptRequirement

inputs:
  IndexInput: File[]
  sampleRecordFiles:
    type:
      type: array
      items:
        type: array
        items: File
  sampleRecordNames: string[]
  isSingle: boolean
  FragmentLength: double?
  StandardDeviation: double?
  BootstrapSamples: int?
  resultsFolder: string

steps:
  index:
    run: kallisto-index.cwl
    in:
      InputFiles: IndexInput
      IndexName:
        source: IndexInput
        valueFrom: $(self[0].nameroot)
    out: [index]

  quant:
    run: kallisto-quant.cwl
    scatter: [InputReads, QuantOutfolder]
    scatterMethod: dotproduct
    in:
      InputReads:
        source: sampleRecordFiles
        valueFrom: $(self)
      QuantOutfolder:
        source: sampleRecordNames
        valueFrom: $(self)
      Index: index/index
      isSingle: isSingle
      FragmentLength: FragmentLength
      StandardDeviation: StandardDeviation
      BootstrapSamples: BootstrapSamples
    out: [kallistoQuantOutDir]
  collectResults:
    run: ./gather-dirs.cwl
    in:
      inDirs: quant/kallistoQuantOutDir
      destinationDir: resultsFolder
    out: [outDir]

outputs:
  kallistoOutDir:
    type: Directory
    outputSource: collectResults/outDir
    
