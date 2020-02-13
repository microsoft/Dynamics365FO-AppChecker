(: Copyright (c) Microsoft Corporation.
   Licensed under the MIT license. :)

(: A best practice check which reports a BPErrorFormCaptionIsEmpty if a form has an empty caption. :)
(: @Category BestPractice/CAR :)
(: @Language Xpp :)
(: @Tags CAR, BP :)
(: @Author pvillads@microsoft.com :)

<BPErrorFormCaptionIsEmpty>
{
    for $f in /Form
    where $f/Metadata/Design[
        Style != 'Dialog' 
    and Style != 'FormPart'
    and Style != 'Lookup'
    and Style != 'Report'
    and UseCaptionFromMenuItem != 'Yes']

    return <Result Artifact='{$f/@Name}'> {$f} </Result>
}
</BPErrorFormCaptionIsEmpty>
