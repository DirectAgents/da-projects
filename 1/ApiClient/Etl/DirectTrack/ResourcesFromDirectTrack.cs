﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using ApiClient.Models.DirectTrack;
using Common;
using DirectTrack;

namespace ApiClient.Etl.DirectTrack
{
    public class ResourcesFromDirectTrack : Source<DirectTrackResource>
    {
        private string url;
        private ResourceGetterMode mode;
        private int limit;
        private DateRange dateRange;
        private ThreadMode threadMode;
        private int batchSize;

        public ResourcesFromDirectTrack(string url,
                                        ResourceGetterMode mode,
                                        int limit = int.MaxValue,
                                        DateRange dateRange = null,
                                        ThreadMode threadMode = ThreadMode.Single,
                                        int batchSize = 10)
        {
            this.url = url;
            this.mode = mode;
            this.limit = limit;
            this.dateRange = dateRange;
            this.threadMode = threadMode;
            this.batchSize = batchSize;
        }

        public override void DoExtract()
        {
            bool containsCampaignID = url.Contains("[campaign_id]");
            bool containsDate = url.Contains("[yyyy]-[mm]-[dd]") || url.Contains("[yyyy]-[mm]");

            if (containsDate && this.dateRange == null)
                throw new Exception("DateRange required when url contains '[yyyy]-[mm]-[dd]'");

            List<string> urlsWithPID = null;

            if (containsCampaignID)
            {
                urlsWithPID = Pids.Select(pid => url.Replace("[campaign_id]", pid.ToString())).ToList();
            }

            Func<string, DateTime, string> substituteDate = (c, date) =>
            {
                return c.Replace("[yyyy]", date.ToString("yyyy"))
                        .Replace("[mm]", date.ToString("MM"))
                        .Replace("[dd]", date.ToString("dd"));
            };

            if (containsCampaignID && containsDate)
            {
                var count = urlsWithPID.Count();
                for (int i = 0; i < (urlsWithPID.Count() / batchSize) + 1; i++)
                {
                    var batch = urlsWithPID.Skip(i * batchSize).Take(batchSize);
                    ForEach(batch, urlWithPID =>
                    {
                        Logger.Log("Working on {0}", urlWithPID);
                        var urls = dateRange.Dates.Select(date => substituteDate(urlWithPID, date)).ToList();
                        Logger.Log("Fetching {0} urls...", urls.Count);
                        DoGetResources(urls);
                    });
                }
            }
            else if (containsCampaignID)
            {
                DoGetResources(urlsWithPID);
            }
            else if (containsDate)
            {
                var urls = dateRange.Dates.Select(date => substituteDate(url, date)).ToList();
                DoGetResources(urls);
            }
            else
            {
                GetResources(url);
            }

            Done = true;
        }

        void ForEach<T>(IEnumerable<T> batch, Action<T> action)
        {
            if (this.threadMode == ThreadMode.Single)
                foreach (var item in batch)
                    action(item);
            else
                Parallel.ForEach(batch, item => action(item));
        }

        private void DoGetResources(List<string> urls)
        {
            urls.ForEach(c =>
            {
                if (Paused)
                    WaitUntilNotPaused();

                GetResources(c);
            });
        }

        private void GetResources(string url)
        {
            var logger = new ConsoleLogger();
            var restCall = new RestCall(new ApiInfo(), logger);
            var getter = new ResourceGetter(logger, url, restCall, limit, this.mode);
            var errors = new List<string>();
            getter.GotResource += (sender, uri, url2, doc, cached) =>
            {
                if (!cached)
                {
                    var newResource = new DirectTrackResource
                    {
                        Name = url2,
                        Content = doc.ToString(),
                        Timestamp = DateTime.UtcNow,
                        AccessId = ApiInfo.LoginAccessId,
                    };

                    newResource.PointsUsed = 10;

                    if (url2.EndsWith("/")) // Resource lists end with '/' and cost 1 point per resourceURL
                    {
                        XNamespace dt = "http://www.digitalriver.com/directtrack/api/resourceList/v1_0";

                        var resourceUrlElements =
                            from c in doc.Root.Elements(dt + "resourceURL")
                            select c;

                        newResource.PointsUsed += resourceUrlElements.Count();
                    }

                    AddItems(new[] { newResource });
                }
            };
            getter.Error += (sender, uri, ex) =>
            {
                Logger.Log("Exception: {0}", ex.Message);
                errors.Add(url + " - " + ex.Message);
            };
            getter.Run();
        }

        int[] Pids
        {
            get
            {
                var pids = PidsStringSales.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(c => int.Parse(c.Trim())).ToArray();
                return pids;
            }
        }


        string PidsStringSales
        {
            get
            {
                return @"4
24
31
44
61
63
65
79
80
87
88
89
91
92
129
137
155
156
163
179
180
311
350
369
407
441
450
499
550
563
589
592
605
635
743
767
788
818
844
892
893
899
909
930
931
949
958
965
976
1014
1075
1095
1110
1122
1215
1226
1263
1266
1332
1445
1476";
            }
        }


        string PidsString2010
        {
            get
            {
                return @"17
31
93
138
184
185
199
214
215
216
223
232
237
248
265
269
273
277
280
291
316
317
328
329
331
345
376
386
394
397
405
410
418
424
446
451
452
454
460
462
463
464
465
472
473
477
481
487
500
506
507
529
542
551
553
556
560
562
566
569
570
572
583
587
590
591
593
594
598
604
606
607
608
625
626
629
630
631
634
636
637
640
644
645
646
648
649
651
679
684
685
687
691
692
706
708
709
710
711
712
715
716
717
721
722
723
725
726
728
729
730
731
733
735
736
737
738
739
740
750
751
752
753
765
767
768
770
771
772
779
782
783
784
787
788
793
794
795
796
797
799
800
801
803
805
806
808
809
810
811
812
817
818
821
823
825
826
833
836
841
842
845
846
852
855
856
857
858
866
867
870
871
874
875
876
879
880
881
882
883
890
891
894
902
903
904
908
911
912
913
917
924
930
931
940
942
944
945
946
948
952
960
961
962
963
964
966
967
968
970
971
972
973
974
975
978
980
982
983
984
986
989
990
991
994
995
996
997
998
999
1000
1002
1003
1004
1005
1006
1008
1010
1011
1012
1014
1015
1016
1017
1018
1020
1021
1022
1023
1024
1025
1026
1027
1028
1030
1031
1033
1034
1035
1037
1038
1039
1040
1042
1043
1044
1045
1046
1047
1048
1049
1050
1051
1053
1054
1055
1057
1058
1059
1060
1061
1062
1064
1065
1066
1067
1068
1069
1072
1075
1077
1078
1079
1080
1081
1082
1083
1084
1085
1086
1087
1088
1089
1090
1091
1093
1094
1095
1096
1097
1098
1100
1101
1103
1104
1106
1107
1108
1109
1112
1115
1120
1123
1124
1125
1127
1128
1129
1130
1131
1132
1135
1136
1138
1139
1140
1141
1142
1143
1144
1146
1148
1149
1150
1151
1153
1154
1155
1156
1157
1158
1159
1160
1162
1163
1164
1166
1167
1168
1169
1170
1171
1172
1173
1174
1175
1176
1178
1179
1180
1181
1182
1183
1184
1186
1187
1188
1190
1191
1192
1193
1194
1195
1197
1199
1200
1201
1202
1204
1205
1206
1207
1208
1209
1210
1211
1212
1213
1214
1215
1216
1217
1220
1222
1223
1226
1227
1228
1230
1231
1233
1234
1235
1236
1237
1238
1239
1240
1241
1242
1243
1244
1245
1246
1248
1249
1250
1251
1252
1253
1254
1255
1256
1258
1259
1260
1261
1262
1264
1269
1270
1275
1276
1277
1278
1279
1280
1281
1282
1283
1284
1285
1286
1287
1288
1289
1290
1291
1293
1294
1298
1299
1302
1303
1306
1310
1316
1317
1318
1319
1320
1321
1322
1324
1325
1326
1327
1329
1330
1332
1334
1337
1338
1340
1341
1342
1344
1349
1350
1351
1352
1353
1354
1356
1357
1358
1359
1360
1361
1363
1364
1365
1366
1367
1370
1371
1372
1373
1374
1375
1376
1379
1380
1381
1382
1383
1386
1387
1388
1389
1390
1391
1392
1393
1394
1395
1397
1398
1399
1404
1405
1406
1407
1409
1410
1412
1414
1418
1419
1420
1423
1426
1427
1431
1434
1436
1439
1440
1441
1443
1446
1447
1448
1449
1451
1454
1456
1457
1458
1459
1460
1467
1468
1474
1489
1490
1491
1502
1503";
            }
        }

        string PidsString2009
        {
            get
            {
                return @"3
14
17
31
55
68
69
86
93
104
105
110
138
171
177
184
185
193
199
208
212
214
215
216
221
223
230
232
237
240
245
248
262
273
277
285
291
292
295
307
308
309
310
311
312
316
317
323
324
325
327
328
329
330
331
334
343
345
346
356
359
360
363
364
365
368
376
377
379
383
386
393
394
397
399
401
403
405
409
410
411
414
416
418
422
424
426
431
432
434
435
437
438
440
442
444
445
446
447
448
451
452
454
455
457
458
460
462
463
464
465
467
468
469
471
477
480
483
484
486
487
488
489
491
494
495
496
498
499
500
501
503
504
505
506
507
508
509
510
512
514
515
516
517
518
521
523
525
529
530
532
533
534
535
536
538
539
540
542
543
544
545
547
549
550
551
553
555
556
557
558
559
560
561
566
567
568
569
570
573
574
575
577
579
580
581
582
587
590
591
593
594
595
596
597
598
600
601
603
604
606
607
608
609
610
611
612
614
615
616
619
620
621
622
624
625
626
627
629
630
631
633
634
635
636
637
638
640
643
644
645
646
647
648
649
650
651
652
653
654
659
660
661
662
663
664
665
667
669
671
672
673
674
675
679
680
681
683
684
685
686
687
688
689
690
691
692
693
694
696
697
698
700
701
705
706
707
708
709
710
711
712
715
716
719
721
722
723
725
726
727
728
730
731
734
735
736
737
738
739
740
742
745
746
747
748
749
750
751
752
753
754
755
756
757
762
763
764
765
766
768
770
771
772
779
780
781
782
783
784
785
786
787
788
790
791
793
794
795
796
797
798
799
800
801
802
803
805
806
807
808
809
810
811
812
817
820
821
825
827
828
829
833
834
836
838
841
842
845
846
848
849
850
851
852
853
855
859
861
864
867
871
873
874
875
876
877
878
880
881
882
886
890
891
894
895
896
897
898
900
902
908
913
916
917
918
920
924
928
929
930
931
932
935
939
940
942
943
945
947
948
952
956
957
960
961
962
963
964
966
967
968
970
972
975
978
980
981
984
986
989
995
996
997
1003
1004
1005
1006
1007
1009
1012
1014
1015";
            }
        }

        string PidsString2008
        {
            get
            {
                return @"3
6
7
8
11
14
16
17
24
28
29
31
44
53
55
62
68
69
81
86
93
94
100
104
105
108
110
125
130
138
157
161
164
166
167
171
172
176
177
181
184
185
192
193
203
206
207
208
217
220
221
222
223
225
226
228
230
232
234
235
236
237
239
240
245
248
250
251
254
256
257
261
262
263
264
265
266
267
268
269
270
271
272
273
277
280
281
282
283
284
285
286
287
288
289
291
292
293
294
295
298
299
300
301
302
303
304
305
306
307
308
309
310
311
312
313
314
315
316
318
319
320
322
323
324
325
326
327
328
329
330
331
334
336
337
338
341
342
343
344
345
346
348
349
350
351
353
354
356
358
359
360
361
362
363
364
365
367
368
371
372
374
375
376
377
378
379
381
382
383
384
385
386
387
389
390
391
393
396
397
398
399
400
401
402
403
404
405
406
407
409
410
411
413
414
416
420
424
426
427
428
429
430
431
433
434
436
437
439
442
443
445
447
448
451
452
453
454
455
458
460
462
463
464
465
467
468
469
476
477
483
494
498
501
503
504
507
508
512
514
515
516
517
518
519
521
522
523
524
525
527
529
531
532
544";
            }
        }

        string PidsString2007
        {
            get
            {
                return @"3
4
6
7
8
9
14
23
24
27
28
29
31
36
43
44
53
61
62
64
69
71
73
74
75
81
86
90
100
104
106
108
109
110
111
125
130
132
133
134
138
140
141
143
151
153
154
155
156
157
158
159
160
161
164
166
167
168
169
171
172
173
175
176
177
178
182
183
184
185
186
187
188
189
191
192
193
194
195
196
197
198
199
200
201
202
203
205
206
207
208
210
217
218
219
220
221
222
224
225
226
227
229
230
231
234
235
236
237
239
240
241
242
243
244
249
251
253
254
257
262
263
264
265
266
267
268
269
270
271
272
273
279
282";
            }
        }

        string PidsString2006
        {
            get
            {
                return @"3
4
6
7
8
9
14
15
16
17
19
21
23
24
26
27
28
29
31
34
37
41
44
48
51
52
53
54
57
58
59
61
62
64
65
69
71
73
74
75
76
79
80
83
86
87
88
89
90
93
94
95
96
97
98
99
100
101
104
105
106
108
109
110
119
121
124
125
126
130
132
133
134
138
139
141";
            }
        }

        string PidsString2005
        {
            get
            {
                return @"3
4
6
7
8
9
10
13
14
15
18
19
20
21
23
24
25
26
27
28
29
31
37
41
44
45
48
49
50
57
59
61
62
64
65
76";
            }
        }
    }
}
