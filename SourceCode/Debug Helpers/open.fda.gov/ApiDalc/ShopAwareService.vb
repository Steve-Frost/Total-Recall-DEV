﻿#Region " Imports "

Imports Newtonsoft.Json.Linq
Imports ApiDalc.DataObjects
Imports ApiDalc.Enumerations
Imports System.ComponentModel

#End Region

Public Class ShopAwareService
    Public Property OpenFdaApiHits As Integer

#Region " Member Variables "

    Private _fda As OpenFda
    Private _restClient As IRestClient

#End Region

#Region " Constructors "

    Public Sub New()
        _restClient = New RestClient

    End Sub

    Public Sub New(restClient As IRestClient)
        _restClient = restClient
    End Sub

#End Region

#Region " Public Methods "


    ''' <summary>
    ''' This Gets a count of Issues. The count is based on Classifications (Class I, Class II, Class III) and drug event
    ''' </summary>
    ''' <param name="keyWord"></param>
    ''' <param name="state"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetSearchSummary(ByVal keyWord As String, ByVal state As String) As SearchSummary

        Dim results As SearchSummary = Nothing

        results = GetRecallInfoCounts(keyWord, state)

        Return results

    End Function

    Public Function GetSearchResult(ByVal keyWord As String, ByVal state As String) As SearchResult

        Const maxResultSetSize As Integer = 3 ' 10

        Dim searchResultLocal As New SearchResult With {.Keyword = keyWord}
        Dim mapList As New Dictionary(Of String, SearchResultMapData)

        'Dim searchSummaryLocal = GetRecallInfoCounts(keyWord, state)

        Dim tmp As List(Of ResultRecall) = GetRecallInfo(keyWord, state, maxResultSetSize)

        'Const testNewCode As Boolean = True

        For Each itm As ResultRecall In tmp

            ProcessResultRecordForMapData(itm, mapList)

            ' ------------------------------------------------------------
            'TODO convert itm (ResultRecall) to SearchResultItem
            ' ------------------------------------------------------------
            
            Dim newItemDate As DateTime = DateTime.ParseExact(itm.Recall_Initiation_Date, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture)
            Dim tmpReportDate As DateTime = DateTime.ParseExact(itm.Report_Date, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture)
            Dim tmpSearchResultItem As New SearchResultItem With {.City = itm.City,
                                                                  .DateStarted = newItemDate.ToShortDateString(),
                                                                  .Content = String.Format("{0} {1}", itm.Reason_For_Recall, itm.Code_info),
                                                                  .DistributionPattern = itm.Distribution_Pattern,
                                                                  .ProductDescription = itm.Product_Description,
                                                                  .State = itm.State,
                                                                  .Status = itm.Status,
                                                                  .Country = itm.Country,
                                                                  .RecallNumber = itm.Recall_Number,
                                                                  .ProductQuantity = itm.Product_Quantity,
                                                                  .EventId = itm.Event_Id,
                                                                  .RecallingFirm = itm.Recalling_Firm,
                                                                  .ReportDate = tmpReportDate.ToShortDateString(),
                                                                  .CodeInfo = itm.Code_info,
                                                                  .Voluntary = itm.Voluntary_Mandated}


            Dim itmDate As DateTime = Nothing
            Select Case itm.Classification

                Case "Class I"

                    'If testNewCode Then
                    searchResultLocal.ClassI.Add(tmpSearchResultItem)
                    'Else
                    '    addSearchResultItemToClassificication(searchResultLocal.ClassI, tmpSearchResultItem, maxResultSetSize)
                    'End If



                    'If searchResultLocal.ClassI.Count < maxResultSetSize Then
                    'searchResultLocal.ClassI.Add(itm)

                    'If searchResultLocal.ClassI.Count = 0 Then
                    '    searchResultLocal.ClassI.Add(tmpSearchResultItem)
                    'Else

                    '    Dim itemAdded = False

                    '    For ndx As Integer = 0 To searchResultLocal.ClassI.Count - 1

                    '        DateTime.TryParse(searchResultLocal.ClassI(ndx).DateStarted, itmDate)

                    '        If newItemDate > itmDate Then

                    '            'searchResultLocal.ClassI.(ndx, tmpSearchResultItem)
                    '            searchResultLocal.ClassI.Insert(ndx, tmpSearchResultItem)
                    '            itemAdded = True
                    '            Exit For

                    '            'Else
                    '            '    If searchResultLocal.ClassI.Count > maxResultSetSize Then
                    '            '    Else
                    '            '        searchResultLocal.ClassI.Add(tmpSearchResultItem)
                    '            '    End If
                    '            '    itemAdded = True
                    '        End If

                    '        'If itemAdded Then
                    '        '    Exit For
                    '        'End If

                    '    Next

                    '    If Not itemAdded AndAlso searchResultLocal.ClassI.Count < maxResultSetSize Then
                    '        searchResultLocal.ClassI.Add(tmpSearchResultItem)
                    '    End If

                    '    'If searchResultLocal.ClassI.Count > maxResultSetSize Then
                    '    '    searchResultLocal.ClassI.RemoveAt(maxResultSetSize)
                    '    'End If


                    '    'If searchResultLocal.ClassI.Count < maxResultSetSize Then

                    '    '    If searchResultLocal.ClassI.Count = 0 Then
                    '    '        searchResultLocal.ClassI.Add(tmpSearchResultItem)
                    '    '    Else

                    '    '        For ndx As Integer = 0 To searchResultLocal.ClassI.Count - 1
                    '    '            DateTime.TryParse(searchResultLocal.ClassI(ndx).DateStarted, itmDate)
                    '    '            'itmDate = DateTime.ParseExact(searchResultLocal.ClassI(ndx).DateStarted, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture)

                    '    '            If newItemDate > itmDate Then

                    '    '                searchResultLocal.ClassI.Insert(ndx, tmpSearchResultItem)
                    '    '                Exit For

                    '    '            End If

                    '    '        Next

                    '    '    End If
                    '    '    ' searchResultLocal.ClassI.Add(tmpSearchResultItem)

                    '    'End If

                    'End If

                    ''End If

                Case "Class II"

                    'If testNewCode Then
                    searchResultLocal.ClassII.Add(tmpSearchResultItem)
                    'Else
                    '    addSearchResultItemToClassificication(searchResultLocal.ClassII, tmpSearchResultItem, maxResultSetSize)
                    'End If


                    'If searchResultLocal.ClassII.Count = 0 Then
                    '    searchResultLocal.ClassII.Add(tmpSearchResultItem)
                    'Else

                    '    Dim itemAdded = False

                    '    For ndx As Integer = 0 To searchResultLocal.ClassII.Count - 1

                    '        DateTime.TryParse(searchResultLocal.ClassII(ndx).DateStarted, itmDate)

                    '        If newItemDate > itmDate Then

                    '            'searchResultLocal.ClassII.(ndx, tmpSearchResultItem)
                    '            searchResultLocal.ClassII.Insert(ndx, tmpSearchResultItem)
                    '            itemAdded = True
                    '            Exit For

                    '        End If

                    '    Next

                    '    If Not itemAdded AndAlso searchResultLocal.ClassII.Count < maxResultSetSize Then
                    '        searchResultLocal.ClassII.Add(tmpSearchResultItem)
                    '    End If

                    'End If

                    ''End If


                    ''If searchResultLocal.ClassII.Count < maxResultSetSize Then
                    ''    'searchResultLocal.ClassII.Add(itm)
                    ''    If searchResultLocal.ClassII.Count < maxResultSetSize Then

                    ''        If searchResultLocal.ClassII.Count = 0 Then
                    ''            searchResultLocal.ClassII.Add(tmpSearchResultItem)
                    ''        Else

                    ''            For ndx As Integer = 0 To searchResultLocal.ClassII.Count - 1
                    ''                DateTime.TryParse(searchResultLocal.ClassII(ndx).DateStarted, itmDate)
                    ''                'itmDate = DateTime.ParseExact(searchResultLocal.ClassII(ndx).DateStarted, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture)

                    ''                If newItemDate > itmDate Then

                    ''                    searchResultLocal.ClassII.Insert(ndx, tmpSearchResultItem)
                    ''                    Exit For

                    ''                End If

                    ''            Next

                    ''        End If
                    ''        ' searchResultLocal.ClassII.Add(tmpSearchResultItem)

                    ''    End If

                    ''End If

                Case "Class III"

                    'If testNewCode Then
                    searchResultLocal.ClassIII.Add(tmpSearchResultItem)
                    'Else
                    '    addSearchResultItemToClassificication(searchResultLocal.ClassIII, tmpSearchResultItem, maxResultSetSize)
                    'End If

                    'If searchResultLocal.ClassIII.Count = 0 Then
                    '    searchResultLocal.ClassIII.Add(tmpSearchResultItem)
                    'Else

                    '    Dim itemAdded = False

                    '    For ndx As Integer = 0 To searchResultLocal.ClassIII.Count - 1

                    '        DateTime.TryParse(searchResultLocal.ClassIII(ndx).DateStarted, itmDate)

                    '        If newItemDate > itmDate Then

                    '            'searchResultLocal.ClassII.(ndx, tmpSearchResultItem)
                    '            searchResultLocal.ClassIII.Insert(ndx, tmpSearchResultItem)
                    '            itemAdded = True
                    '            Exit For

                    '        End If

                    '    Next

                    '    If Not itemAdded AndAlso searchResultLocal.ClassIII.Count < maxResultSetSize Then
                    '        searchResultLocal.ClassIII.Add(tmpSearchResultItem)
                    '    End If

                    'End If


                    ''If searchResultLocal.ClassIII.Count < maxResultSetSize Then
                    ''    'searchResultLocal.ClassIII.Add(itm)
                    ''    If searchResultLocal.ClassIII.Count < maxResultSetSize Then

                    ''        If searchResultLocal.ClassIII.Count = 0 Then
                    ''            searchResultLocal.ClassIII.Add(tmpSearchResultItem)
                    ''        Else

                    ''            For ndx As Integer = 0 To searchResultLocal.ClassIII.Count - 1
                    ''                DateTime.TryParse(searchResultLocal.ClassIII(ndx).DateStarted, itmDate)
                    ''                'itmDate = DateTime.ParseExact(searchResultLocal.ClassIII(ndx).DateStarted, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture)

                    ''                If newItemDate > itmDate Then

                    ''                    searchResultLocal.ClassIII.Insert(ndx, tmpSearchResultItem)
                    ''                    Exit For

                    ''                End If

                    ''            Next

                    ''        End If
                    ''        ' searchResultLocal.ClassIII.Add(tmpSearchResultItem)

                    ''    End If

                    ''End If

            End Select

        Next

        searchResultLocal.MapObjects = ConvertDictionaryMapObjectsToSearchResult(mapList)

        'Dim sw As New Stopwatch
        'sw.Start()

        'Dim tmpLinqResults = (From el In searchResultLocal.ClassI Select el Order By CDate(el.ReportDate) Descending).ToList()
        'sw.Stop()
        'Debug.Write(sw.Elapsed)

        If searchResultLocal.ClassI.Count > maxResultSetSize Then

            'If testNewCode Then

            Dim tmpLinqResults = (From el In searchResultLocal.ClassI Select el Order By CDate(el.ReportDate) Descending).ToList()
            tmpLinqResults.RemoveRange(maxResultSetSize, tmpLinqResults.Count - maxResultSetSize)

            searchResultLocal.ClassI.Clear()
            searchResultLocal.ClassI.AddRange(tmpLinqResults)

            'Else
            '    searchResultLocal.ClassI.RemoveRange(maxResultSetSize, searchResultLocal.ClassI.Count - maxResultSetSize)
            'End If

        End If

        If searchResultLocal.ClassII.Count > maxResultSetSize Then

            'If testNewCode Then

            Dim tmpLinqResults = (From el In searchResultLocal.ClassII Select el Order By CDate(el.ReportDate) Descending).ToList()
            tmpLinqResults.RemoveRange(maxResultSetSize, tmpLinqResults.Count - maxResultSetSize)

            searchResultLocal.ClassII.Clear()
            searchResultLocal.ClassII.AddRange(tmpLinqResults)
            'Else
            '    searchResultLocal.ClassII.RemoveRange(maxResultSetSize, searchResultLocal.ClassII.Count - maxResultSetSize)
            'End If

        End If

        If searchResultLocal.ClassIII.Count > maxResultSetSize Then

            'If testNewCode Then

            Dim tmpLinqResults = (From el In searchResultLocal.ClassIII Select el Order By CDate(el.ReportDate) Descending).ToList()
            tmpLinqResults.RemoveRange(maxResultSetSize, tmpLinqResults.Count - maxResultSetSize)

            searchResultLocal.ClassIII.Clear()
            searchResultLocal.ClassIII.AddRange(tmpLinqResults)

            'Else
            '    searchResultLocal.ClassIII.RemoveRange(maxResultSetSize, searchResultLocal.ClassIII.Count - maxResultSetSize)
            'End If

        End If

        Return searchResultLocal

    End Function

    ' ''' <summary>
    ' ''' This gets the Top recall result for the KeyWord
    ' ''' </summary>
    ' ''' <param name="keyWordList"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function GetRecallsSummary(ByVal keyWordList As List(Of String)) As List(Of RecallSearchResultData)

    '    Dim results As List(Of RecallSearchResultData)

    '    results = GetRecallInfo(keyWordList, 0)

    '    Return results

    'End Function

    ' ''' <summary>
    ' ''' Returns the 
    ' ''' </summary>
    ' ''' <param name="keyWord"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function GetRecallsDetail(ByVal keyWord As String) As List(Of RecallSearchResultData)

    '    Dim results As List(Of RecallSearchResultData)
    '    Dim keyWordList As New List(Of String)

    '    keyWordList.Add(keyWord)

    '    results = GetRecallInfo(keyWordList, 100)

    '    Return results

    'End Function

#End Region

#Region " Private Methods "

    Private Function GetRecallInfoCounts(keyWord As String, state As String) As SearchSummary

        _fda = New OpenFda(_restClient)

        'TODO: Need to query Drug/Events

        Dim searchSummaryForKeyword As New SearchSummary With {.Keyword = keyWord}
        Dim filterType As FdaFilterTypes
        filterType = FdaFilterTypes.RecallReason

        Dim endPointList As New List(Of OpenFdaApiEndPoints)({OpenFdaApiEndPoints.FoodRecall, OpenFdaApiEndPoints.DrugRecall, OpenFdaApiEndPoints.DeviceRecall})

        Const maxresultsize As Integer = 0

        Dim filterList As New List(Of String)
        filterList.Add(keyWord)

        'Dim recallResultList As List(Of ResultRecall)

        For Each endPoint In endPointList

            'recallResultList = New List(Of ResultRecall)
            Dim endpointSearchSummary As SearchSummary = ExecuteSearchCounts(endPoint, filterType, filterList, maxresultsize, state, "classification")

            If endpointSearchSummary IsNot Nothing Then

                searchSummaryForKeyword.ClassICount += endpointSearchSummary.ClassICount
                searchSummaryForKeyword.ClassIICount += endpointSearchSummary.ClassIICount
                searchSummaryForKeyword.ClassIIICount += endpointSearchSummary.ClassIIICount

                ' searchSummaryForKeyword.EventCount += endpointSearchSummary.EventCount

            End If

        Next

        Return searchSummaryForKeyword

    End Function

    Private Function GetRecallInfo(ByVal keyWord As String, state As String, resultSize As Integer) As List(Of ResultRecall)

        OpenFdaApiHits = 0

        _fda = New OpenFda(_restClient)

        resultSize = 100

        Dim apiUrl As String = String.Empty
        Dim searchResults As String
        Dim resultList As New List(Of ResultRecall)

        Dim endPointList As New List(Of OpenFdaApiEndPoints)({OpenFdaApiEndPoints.FoodRecall, OpenFdaApiEndPoints.DrugRecall, OpenFdaApiEndPoints.DeviceRecall})

        Dim classificationList As New List(Of String)({"Class I", "Class II", "Class III"})

        For Each endPointType In endPointList

            For Each cc In classificationList

                Dim filterList As New List(Of String)({state})

                'Limit first query to a 1 year window

                Dim beginDate As String = String.Format("{0:yyyyMMdd}", DateTime.Now.AddDays(1))
                Dim endDate As String = String.Format("{0:yyyyMMdd}", DateTime.Now.AddYears(-1))

                _fda.ResetSearch()
                _fda.AddSearchFilter(endPointType, FdaFilterTypes.Region, filterList, FilterCompairType.And)
                _fda.AddSearchFilter(endPointType, FdaFilterTypes.RecallReason, New List(Of String)({keyWord}), FilterCompairType.And)
                _fda.AddSearchFilter(endPointType, FdaFilterTypes.Date, New List(Of String)({beginDate, endDate}), FilterCompairType.And)
                _fda.AddSearchFilter(endPointType, "classification", cc, FilterCompairType.And)

                apiUrl = _fda.BuildUrl(endPointType, resultSize)

                searchResults = _fda.Execute(apiUrl)
                OpenFdaApiHits += 1

                Dim dataSetSize As Integer = _fda.GetMetaResults().Total()
                
                ' If there was not data in the 1 yr window the get all results.
                ' Check a 2 yr window for results.
                If dataSetSize = 0 Then


                    'beginDate = String.Format("{0:yyyyMMdd}", DateTime.Now.AddDays(1))
                    endDate = String.Format("{0:yyyyMMdd}", DateTime.Now.AddYears(-2))

                    _fda.ResetSearch()
                    _fda.AddSearchFilter(endPointType, FdaFilterTypes.Region, filterList, FilterCompairType.And)
                    _fda.AddSearchFilter(endPointType, FdaFilterTypes.RecallReason, New List(Of String)({keyWord}), FilterCompairType.And)
                    _fda.AddSearchFilter(endPointType, FdaFilterTypes.Date, New List(Of String)({beginDate, endDate}), FilterCompairType.And)
                    _fda.AddSearchFilter(endPointType, "classification", cc, FilterCompairType.And)

                    apiUrl = _fda.BuildUrl(endPointType, resultSize)

                    searchResults = _fda.Execute(apiUrl)
                    OpenFdaApiHits += 1

                    dataSetSize = _fda.GetMetaResults().Total()

                End If

                ' If there was not data in the 2 yr window the get all results.
                If dataSetSize = 0 Then

                    _fda.ResetSearch()
                    _fda.AddSearchFilter(endPointType, FdaFilterTypes.Region, filterList, FilterCompairType.And)
                    _fda.AddSearchFilter(endPointType, FdaFilterTypes.RecallReason, New List(Of String)({keyWord}), FilterCompairType.And)
                    _fda.AddSearchFilter(endPointType, "classification", cc, FilterCompairType.And)

                    apiUrl = _fda.BuildUrl(endPointType, resultSize)

                    searchResults = _fda.Execute(apiUrl)
                    OpenFdaApiHits += 1

                    dataSetSize = _fda.GetMetaResults().Total()

                End If

                ''Check SearchResults  meta.Results.Total
                '' if count is 0 then remove Date range and try again
                'Dim isPagingRequired As Boolean = False

                'If dataSetSize > 100 Then
                '    isPagingRequired = True
                'End If

                ''Do
                ''    '    [ statements ]
                ''    '    [ Continue Do ]
                ''    '    [ statements ]
                ''    '    [ Exit Do ]
                ''    '    [ statements ]
                ''    'Loop { While | Until } condition
                ''Loop(ispagingrequired
                ''    )

                ''Do
                ''    'Debug.Write(index.ToString & " ")
                ''    'index += 1
                ''Loop Until Not isPagingRequired

                ' if total records int the Search request exceeds the max of 100 records per request
                ' then page through the data
                ' LIMIT the number of page request to a MAX of 5
                Dim pageLimit As Integer = CInt(Decimal.Ceiling(dataSetSize / 100))
                If pageLimit > 5 Then
                    pageLimit = 5
                End If

                Dim skipValue As Integer = 0
                If dataSetSize > 0 Then

                    Do
                        pageLimit -= 1

                        If Not String.IsNullOrEmpty(searchResults) Then

                            Dim result As List(Of ResultRecall) = ResultRecall.CnvJsonDataToList(searchResults)
                            resultList.AddRange(result)

                        End If

                        If pageLimit > 0 Then

                            skipValue += 100
                            Dim newApiUrl As String = apiUrl.Replace("&limit=100", String.Format("&limit=100&skip={0}", skipValue))
                            searchResults = _fda.Execute(apiUrl)
                            OpenFdaApiHits += 1

                        End If

                    Loop Until pageLimit = 0

                End If

            Next

        Next

        Return resultList

        'tmpRecallResultList = ResultRecall.CnvJsonDataToList(searchResults)

    End Function

    Private Function GetRecallInfo(ByVal keyWordList As List(Of String), ByVal maxresultsize As Integer) As List(Of RecallSearchResultData)

        _fda = New OpenFda(_restClient)

        Dim results As New List(Of RecallSearchResultData)

        Dim filterType As FdaFilterTypes
        filterType = FdaFilterTypes.RecallReason

        Dim resultCount As Integer
        Dim recallResultList As New List(Of ResultRecall)

        For Each kwGroup In keyWordList

            Dim filterList As New List(Of String)
            Dim kwGroupArray As String() = kwGroup.Split(",")

            For Each itm In kwGroupArray
                filterList.Add(itm)
            Next

            Dim endPointList As New List(Of OpenFdaApiEndPoints)({OpenFdaApiEndPoints.FoodRecall, OpenFdaApiEndPoints.DrugRecall, OpenFdaApiEndPoints.DeviceRecall})

            For Each endPoint In endPointList

                recallResultList = New List(Of ResultRecall)
                resultCount = ExecuteSearch(endPoint, filterType, filterList, maxresultsize, recallResultList)

                For Each itm As ResultRecall In recallResultList

                    Dim itmClassification As Classification

                    Select Case itm.Classification
                        Case "Class I"
                            itmClassification = Classification.Class_I

                        Case "Class II"
                            itmClassification = Classification.Class_II

                        Case "Class III"
                            itmClassification = Classification.Class_III

                    End Select

                    'itm.KeyWord = kwGroup

                    Dim recallData As New RecallSearchResultData With {.KeyWord = itm.KeyWord,
                                                                       .Type = itm.Product_Type,
                                                                       .Count = resultCount,
                                                                       .Classification = String.Format("{0}  -  {1}", itm.Classification, GetEnumDescription(itmClassification)),
                                                                       .ProductDescription = itm.Product_Description,
                                                                       .ReasonForRecall = itm.Reason_For_Recall}

                    RecallData_AddPropertyInfo(recallData, itm)
                    results.Add(recallData)

                Next

            Next

        Next

        Return results

    End Function

    Private Function ExecuteSearch(endPointType As OpenFdaApiEndPoints, filterType As FdaFilterTypes, filterList As List(Of String), ByVal maxresultsize As Integer, ByRef recallResultList As List(Of ResultRecall)) As Integer

        Dim apiUrl As String = String.Empty
        Dim searchResults As String
        Dim srMetaData As MetaResults
        Dim tmpRecallResultList As New List(Of ResultRecall)

        _fda.AddSearchFilter(endPointType, filterType, filterList)
        apiUrl = _fda.BuildUrl(endPointType, maxresultsize)
        searchResults = _fda.Execute(apiUrl)

        srMetaData = MetaResults.CnvJsonData(searchResults)

        If srMetaData.Total > 0 Then

            tmpRecallResultList = ResultRecall.CnvJsonDataToList(searchResults)

            If tmpRecallResultList.Count > 0 Then

                For Each itm As ResultRecall In tmpRecallResultList

                    itm.KeyWord = filterList(0)
                    recallResultList.Add(itm)

                Next

            End If

        End If

        Return srMetaData.Total

    End Function

    Private Function ExecuteSearchCounts(endPointType As OpenFdaApiEndPoints, filterType As FdaFilterTypes, filterList As List(Of String), ByVal maxresultsize As Integer, ByVal state As String, ByVal cntField As String) As SearchSummary

        Dim apiUrl As String = String.Empty
        Dim tmpRecallResultList As New List(Of ResultRecall)

        Dim searchSummary As New SearchSummary With {.Keyword = filterList(0)}

        _fda.AddSearchFilter(endPointType, FdaFilterTypes.Region, New List(Of String)({state}), FilterCompairType.And)
        _fda.AddSearchFilter(endPointType, filterType, filterList, FilterCompairType.And)

        apiUrl = _fda.BuildUrl(endPointType, maxresultsize)
        apiUrl += String.Format("&count={0}.exact", cntField.ToLower)

        Dim searchResults As String = _fda.Execute(apiUrl)

        If Not String.IsNullOrEmpty(searchResults) Then

            Dim jo As JObject = JObject.Parse(searchResults)
            Dim countResults As JArray = jo("results")

            Dim termCountFound As Boolean = False
            Dim termCount As Integer

            For Each itm In countResults

                termCount = itm("count")

                Select Case itm("term")
                    Case "Class I"
                        searchSummary.ClassICount = termCount
                        termCountFound = True

                    Case "Class II"
                        searchSummary.ClassIICount = termCount
                        termCountFound = True


                    Case "Class III"
                        searchSummary.ClassIIICount = termCount
                        termCountFound = True

                End Select

            Next

            If Not termCountFound Then
                searchSummary = Nothing
            End If

        End If

        Return searchSummary

    End Function

    Private Sub RecallData_AddPropertyInfo(ByRef recallData As RecallSearchResultData, ByVal itm As ResultRecall)

        If itm.Distribution_Pattern.ToLower.Contains("nationwide") Then
            recallData.IsNationWide = True
        End If

        If Not String.IsNullOrEmpty(itm.State) Then
            recallData.Regions.Add(itm.State)
        End If

        Dim items As Array

        items = System.Enum.GetValues(GetType(States))

        Dim tmpState As States

        For Each item As String In items

            tmpState = DirectCast([Enum].Parse(GetType(States), item), States)

            If itm.Distribution_Pattern.Contains(tmpState.ToString) OrElse
                itm.Distribution_Pattern.Contains(GetEnumDescription(tmpState)) OrElse
                recallData.IsNationWide Then

                recallData.Regions.Add(tmpState.ToString)

            End If

        Next

    End Sub

    Private Sub ProcessResultRecordForMapData(data As ResultRecall, ByRef list As Dictionary(Of String, SearchResultMapData))

        Dim check As String = data.Distribution_Pattern
        Dim states As List(Of String) = System.Enum.GetNames(GetType(States)).ToList
        Dim nationwide As Boolean = False

        Try

            If check.ToLower.Contains("nationwide") Then
                nationwide = True
            End If

            For Each state As String In states

                Dim stEnum As Reflection.FieldInfo = GetType(States).GetField(state)
                Dim stateName As DescriptionAttribute = DirectCast(stEnum.GetCustomAttributes(GetType(DescriptionAttribute), False)(0), DescriptionAttribute)
                Dim stateCoords As DefaultValueAttribute = DirectCast(stEnum.GetCustomAttributes(GetType(DefaultValueAttribute), False)(0), DefaultValueAttribute)
                Dim coordPair As List(Of String) = stateCoords.Value.ToString.Split(";").ToList

                If check.Contains(state) Or nationwide Then

                    Dim listCheck As SearchResultMapData = Nothing

                    If list.ContainsKey(state) Then
                        listCheck = list(state)
                    Else

                        listCheck = New SearchResultMapData With {.State = state, .Latitude = coordPair(0), .Longitude = coordPair(1)}
                        list.Add(state, listCheck)

                    End If

                    Dim tooltip As String = String.Concat(data.Product_Type, " {0}")

                    Select Case data.Classification.ToLower

                        Case "class i"

                            tooltip = String.Format(tooltip, " Class-1")
                            listCheck.IconSet = (listCheck.IconSet Or IconSet.Class1)

                        Case "class ii"

                            tooltip = String.Format(tooltip, " Class-2")
                            listCheck.IconSet = (listCheck.IconSet Or IconSet.Class2)

                        Case "class iii"

                            tooltip = String.Format(tooltip, " Class-3")
                            listCheck.IconSet = (listCheck.IconSet Or IconSet.Class3)

                    End Select

                    If Not listCheck.Tooltip.Contains(tooltip) Then

                        If listCheck.Tooltip.Length > 0 Then
                            listCheck.Tooltip += ", "
                        End If

                        listCheck.Tooltip += tooltip

                    End If

                    list(state) = listCheck

                End If

            Next

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Function ConvertDictionaryMapObjectsToSearchResult(mapList As Dictionary(Of String, SearchResultMapData)) As List(Of SearchResultMapData)

        Dim result As New List(Of SearchResultMapData)

        For Each mapData As KeyValuePair(Of String, SearchResultMapData) In mapList
            result.Add(mapData.Value)
        Next

        Return result

    End Function

    Private Sub addSearchResultItemToClassificication(searchResultList As List(Of SearchResultItem), tmpSearchResultItem As SearchResultItem, maxResultSetSize As Integer)

        Dim itmDate As DateTime
        Dim newItemDate As DateTime

        DateTime.TryParse(tmpSearchResultItem.DateStarted, newItemDate)

        If searchResultList.Count = 0 Then
            searchResultList.Add(tmpSearchResultItem)
        Else

            Dim itemAdded = False

            For ndx As Integer = 0 To searchResultList.Count - 1

                DateTime.TryParse(searchResultList(ndx).DateStarted, itmDate)

                If newItemDate > itmDate Then

                    searchResultList.Insert(ndx, tmpSearchResultItem)
                    itemAdded = True
                    Exit For

                End If

            Next

            If Not itemAdded AndAlso searchResultList.Count < maxResultSetSize Then
                searchResultList.Add(tmpSearchResultItem)
            End If

        End If

    End Sub

#End Region

End Class
