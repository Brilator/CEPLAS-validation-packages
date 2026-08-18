#!/usr/bin/env cwl-runner
cwlVersion: v1.2
class: Workflow

requirements:
  - class: SubworkflowFeatureRequirement
  - class: MultipleInputFeatureRequirement

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
  kallisto:
    run:  ../../workflows/kallisto/workflow.cwl
    in:
      IndexInput: IndexInput
      sampleRecordFiles: sampleRecordFiles
      sampleRecordNames: sampleRecordNames
      isSingle: isSingle
      FragmentLength: FragmentLength
      StandardDeviation: StandardDeviation
      BootstrapSamples: BootstrapSamples
      resultsFolder: resultsFolder
    out: [kallistoOutDir]

outputs:
  kallistoOutDir:
    type: Directory
    outputSource: kallisto/kallistoOutDir

$namespaces:
  s: https://schema.org/
  edam: http://edamontology.org/

$schemas:
  - https://schema.org/version/latest/schemaorg-current-https.rdf
  - http://edamontology.org/EDAM_1.18.owl

s:author:
  - class: s:Person
    s:name: Dominik Brilhaus
    s:identifier: https://orcid.org/0000-0001-9021-3197