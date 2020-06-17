
var pieceLengths = "";


/////////////Result matrixes

$(document).ready(function () {
    $("#btnsubmit").click(function (e) {
        //Serialize the form datas.   
        var valdata = $("#mainForm").serialize();
        var data = JSON.stringify({
            geoinfotmationModelingViewModel: valdata,
        });
        //to get alert popup   
        $.ajax({
            url: "/Calculate",
            type: "POST",
            contentType: 'application/json',
            data: data
        });
    });

    $('#saveDataRiver').click(function (e) {
        //Serialize the form datas.   
        var valdata = $("#mainForm").serialize();
        var data = JSON.stringify({
            geoinfotmationModelingViewModel: valdata,
        });
        //to get alert popup   
        $.ajax({
            url: "/Home/SaveRiverData",
            type: "POST",
            contentType: "application/x-www-form-urlencoded; charset=UTF-8",
            data: data,
            success: function (result) {
                $('#saveData').modal('hide');
            },
        });
    });


  
});

function GetHtmlTableFoeMatrix(matrix) {
    var res = "<thead class='thead-dark'><tr><th scope='col'>#</th>";
    for (var k = 0; k < matrix[0].length; k++) {
        res += "<th scope='col'>" + k + "</th>";
    }
    res += "</tr></thread><tbody>";
    for (var i = 0; i < matrix.length; i++) {
        res += "<tr><th scope='row'>" + i + "</th>";
        for (var j = 0; j < matrix[i].length; j++) {
            res += "<td>" + matrix[i][j] + "</td>";
        }
        res += "</tr>"
    }
    res += "</tbody>";
    return res;
}


function resultMatrixes(data, status, xhr) {
    var matrixH = data.result.matrixH;
    var matrixU = data.result.matrixU;

    $(".table-matrix-h").html(GetHtmlTableFoeMatrix(matrixH));
    $(".table-matrix-u").html(GetHtmlTableFoeMatrix(matrixU));

    $(".btn-matrixes").removeClass("d-none");
    $(".charts-modals").html("");
    $(".js-buttons-open-charts").html("");
    $('.js-time-charts').html("");
    var isFirst = true;
    var resultData = data.result.mapDetails;
    for (var i = 0; i < resultData.length; i++) {






        $('#leg' + i + ' .btn').removeClass('d-none');
        if (isFirst) {
            for (var m = 0; m < resultData[0].matrixH.length; m++) {
                $(".js-buttons-open-charts").append("<button type='button' value='" + m + "' class=' time-btn btn btn-primary mt-2 mr-2'  id='" + "time-" + m + "'> " + "t " + m + "</button>");
                $('.js-time-charts').append('<div id="test' + m + 'chartdivaria" class="js-test-charts d-none" style="height: 500px;"></div>');
                $('.js-time-charts').append('<div id="test' + m + 'chartdivspeed" class="js-test-charts d-none" style="height: 500px;"></div>');

                am4core.ready(function () {

                    // Themes begin
                    am4core.useTheme(am4themes_animated);
                    // Themes end




                    // Create chart instance
                    var chart = am4core.create("test" + m + "chartdivaria", am4charts.XYChart);
                    // Create axes
                    var dateAxis = chart.xAxes.push(new am4charts.ValueAxis());
                    var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
                    var details = data.result.mapDetails;
                    var countH = details.length;

                    for (var j = 0; j < countH; j++) {
                        createSeries("value" + j, "Leg " + j, j);
                    }

                    // Create series
                    function createSeries(s, name, index) {
                        var series = chart.series.push(new am4charts.LineSeries());
                        series.dataFields.valueY = "value" + s;
                        series.dataFields.valueX = "valueX";
                        series.name = name;

                        var segment = series.segments.template;
                        segment.interactionsEnabled = true;

                        var hoverState = segment.states.create("hover");
                        hoverState.properties.strokeWidth = 3;

                        var dimmed = segment.states.create("dimmed");
                        dimmed.properties.stroke = am4core.color("#dadada");

                        segment.events.on("over", function (event) {
                            processOver(event.target.parent.parent.parent);
                        });

                        segment.events.on("out", function (event) {
                            processOut(event.target.parent.parent.parent);
                        });

                        var data = [];
                        var value = 0;
                        for (var j = 1; j < details[index].matrixH[m].length; j++) {
                            value = details[index].matrixH[m][j];
                            var dataItem = { valueX: j };
                            dataItem["value" + s] = value;
                            data.push(dataItem);
                        }

                        series.data = data;
                        return series;
                    }

                    chart.legend = new am4charts.Legend();
                    chart.legend.position = "right";
                    chart.legend.scrollable = true;
                    chart.legend.itemContainers.template.events.on("over", function (event) {
                        processOver(event.target.dataItem.dataContext);
                    })

                    chart.legend.itemContainers.template.events.on("out", function (event) {
                        processOut(event.target.dataItem.dataContext);
                    })

                    function processOver(hoveredSeries) {
                        hoveredSeries.toFront();

                        hoveredSeries.segments.each(function (segment) {
                            segment.setState("hover");
                        })

                        chart.series.each(function (series) {
                            if (series != hoveredSeries) {
                                series.segments.each(function (segment) {
                                    segment.setState("dimmed");
                                })
                                series.bulletsContainer.setState("dimmed");
                            }
                        });
                    }

                    function processOut(hoveredSeries) {
                        chart.series.each(function (series) {
                            series.segments.each(function (segment) {
                                segment.setState("default");
                            })
                            series.bulletsContainer.setState("default");
                        });
                    }

                }); // end am4core.ready()

                am4core.ready(function () {

                    // Themes begin
                    am4core.useTheme(am4themes_animated);
                    // Themes end




                    // Create chart instance
                    var chart = am4core.create('test' + m + 'chartdivspeed', am4charts.XYChart);
                    // Create axes
                    var dateAxis = chart.xAxes.push(new am4charts.ValueAxis());
                    var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());

                    var details = data.result.mapDetails;
                    var countU = details.length;
                    for (var j = 0; j < countU; j++) {
                        createSeries("value" + j, "Leg " + j, j);
                    }

                    // Create series
                    function createSeries(s, name, index) {
                        var series = chart.series.push(new am4charts.LineSeries());
                        series.dataFields.valueY = "value" + s;
                        series.dataFields.valueX = "valueX";
                        series.name = name;

                        var segment = series.segments.template;
                        segment.interactionsEnabled = true;

                        var hoverState = segment.states.create("hover");
                        hoverState.properties.strokeWidth = 3;

                        var dimmed = segment.states.create("dimmed");
                        dimmed.properties.stroke = am4core.color("#dadada");

                        segment.events.on("over", function (event) {
                            processOver(event.target.parent.parent.parent);
                        });

                        segment.events.on("out", function (event) {
                            processOut(event.target.parent.parent.parent);
                        });

                        var data = [];
                        var value = 0;
                        for (var j = 1; j < details[index].matrixU[m].length; j++) {
                            value = details[index].matrixU[m][j];
                            var dataItem = { valueX: j };
                            dataItem["value" + s] = value;
                            data.push(dataItem);
                        }

                        series.data = data;
                        return series;
                    }

                    chart.legend = new am4charts.Legend();
                    chart.legend.position = "right";
                    chart.legend.scrollable = true;
                    chart.legend.itemContainers.template.events.on("over", function (event) {
                        processOver(event.target.dataItem.dataContext);
                    })

                    chart.legend.itemContainers.template.events.on("out", function (event) {
                        processOut(event.target.dataItem.dataContext);
                    })

                    function processOver(hoveredSeries) {
                        hoveredSeries.toFront();

                        hoveredSeries.segments.each(function (segment) {
                            segment.setState("hover");
                        })

                        chart.series.each(function (series) {
                            if (series != hoveredSeries) {
                                series.segments.each(function (segment) {
                                    segment.setState("dimmed");
                                })
                                series.bulletsContainer.setState("dimmed");
                            }
                        });
                    }

                    function processOut(hoveredSeries) {
                        chart.series.each(function (series) {
                            series.segments.each(function (segment) {
                                segment.setState("default");
                            })
                            series.bulletsContainer.setState("default");
                        });
                    }

                }); // end am4core.ready()
            }
            isFirst = false;
        }

        $(".charts-modals").append("<div class='modal' id='modalleg" + i + "' tabindex=' - 1' role='dialog' aria-labelledby='modalleg" + i + "' aria-hidden='true'>" +
            "<div class='modal-dialog modal-lg' role='document'>" +
            " <div class='modal-content'>" +
            ' <div class="modal-header">' +
            ' <button type="button" class="close" data-dismiss="modal" aria-label="Close">' +
            '<span aria-hidden="true">&times;</span>' +
            '</button> </div ><div class="modal-body">' + '<div id="chartarialeg' + i + '" style="height: 500px;"></div>' + '<div id="chartspeedleg' + i + '" style="height: 500px;"></div>' +
            ' </div><div class= "modal-footer"><button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button></div></div></div></div> ');

        am4core.ready(function () {

            // Themes begin
            am4core.useTheme(am4themes_animated);
            // Themes end




            // Create chart instance
            var chart = am4core.create("chartarialeg" + i, am4charts.XYChart);
            // Create axes
            var dateAxis = chart.xAxes.push(new am4charts.ValueAxis());
            var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
            var pieceMatrixH = data.result.mapDetails[i].matrixH;
            var countH = pieceMatrixH.length;

            for (var j = 0; j < countH; j++) {
                createSeries("value" + j, "t = " + j, j);
            }

            // Create series
            function createSeries(s, name, index) {
                var series = chart.series.push(new am4charts.LineSeries());
                series.dataFields.valueY = "value" + s;
                series.dataFields.valueX = "valueX";
                series.name = name;

                var segment = series.segments.template;
                segment.interactionsEnabled = true;

                var hoverState = segment.states.create("hover");
                hoverState.properties.strokeWidth = 3;

                var dimmed = segment.states.create("dimmed");
                dimmed.properties.stroke = am4core.color("#dadada");

                segment.events.on("over", function (event) {
                    processOver(event.target.parent.parent.parent);
                });

                segment.events.on("out", function (event) {
                    processOut(event.target.parent.parent.parent);
                });

                var data = [];
                var value = 0;
                for (var j = 1; j < pieceMatrixH[index].length; j++) {
                    value = pieceMatrixH[index][j];
                    var dataItem = { valueX: j };
                    dataItem["value" + s] = value;
                    data.push(dataItem);
                }

                series.data = data;
                return series;
            }

            chart.legend = new am4charts.Legend();
            chart.legend.position = "right";
            chart.legend.scrollable = true;
            chart.legend.itemContainers.template.events.on("over", function (event) {
                processOver(event.target.dataItem.dataContext);
            })

            chart.legend.itemContainers.template.events.on("out", function (event) {
                processOut(event.target.dataItem.dataContext);
            })

            function processOver(hoveredSeries) {
                hoveredSeries.toFront();

                hoveredSeries.segments.each(function (segment) {
                    segment.setState("hover");
                })

                chart.series.each(function (series) {
                    if (series != hoveredSeries) {
                        series.segments.each(function (segment) {
                            segment.setState("dimmed");
                        })
                        series.bulletsContainer.setState("dimmed");
                    }
                });
            }

            function processOut(hoveredSeries) {
                chart.series.each(function (series) {
                    series.segments.each(function (segment) {
                        segment.setState("default");
                    })
                    series.bulletsContainer.setState("default");
                });
            }

        }); // end am4core.ready()

        am4core.ready(function () {

            // Themes begin
            am4core.useTheme(am4themes_animated);
            // Themes end




            // Create chart instance
            var chart = am4core.create("chartspeedleg" + i, am4charts.XYChart);
            // Create axes
            var dateAxis = chart.xAxes.push(new am4charts.ValueAxis());
            var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());

            var pieceMatrixU = data.result.mapDetails[i].matrixU;
            var countU = pieceMatrixU.length;
            for (var j = 0; j < countU; j++) {
                createSeries("value" + j, "t =" + j, j);
            }

            // Create series
            function createSeries(s, name, index) {
                var series = chart.series.push(new am4charts.LineSeries());
                series.dataFields.valueY = "value" + s;
                series.dataFields.valueX = "valueX";
                series.name = name;

                var segment = series.segments.template;
                segment.interactionsEnabled = true;

                var hoverState = segment.states.create("hover");
                hoverState.properties.strokeWidth = 3;

                var dimmed = segment.states.create("dimmed");
                dimmed.properties.stroke = am4core.color("#dadada");

                segment.events.on("over", function (event) {
                    processOver(event.target.parent.parent.parent);
                });

                segment.events.on("out", function (event) {
                    processOut(event.target.parent.parent.parent);
                });

                var data = [];
                var value = 0;
                for (var j = 1; j < pieceMatrixU[index].length; j++) {
                    value = pieceMatrixU[index][j];
                    var dataItem = { valueX: j };
                    dataItem["value" + s] = value;
                    data.push(dataItem);
                }

                series.data = data;
                return series;
            }

            chart.legend = new am4charts.Legend();
            chart.legend.position = "right";
            chart.legend.scrollable = true;
            chart.legend.itemContainers.template.events.on("over", function (event) {
                processOver(event.target.dataItem.dataContext);
            })

            chart.legend.itemContainers.template.events.on("out", function (event) {
                processOut(event.target.dataItem.dataContext);
            })

            function processOver(hoveredSeries) {
                hoveredSeries.toFront();

                hoveredSeries.segments.each(function (segment) {
                    segment.setState("hover");
                })

                chart.series.each(function (series) {
                    if (series != hoveredSeries) {
                        series.segments.each(function (segment) {
                            segment.setState("dimmed");
                        })
                        series.bulletsContainer.setState("dimmed");
                    }
                });
            }

            function processOut(hoveredSeries) {
                chart.series.each(function (series) {
                    series.segments.each(function (segment) {
                        segment.setState("default");
                    })
                    series.bulletsContainer.setState("default");
                });
            }

        }); // end am4core.ready()


        $(".time-btn").click(function (e) {
            $('.js-test-charts').addClass('d-none');
            $('.time-btn').removeClass('btn-secondary');
            $(this).addClass('btn-secondary');
            var i = $(this).val();
            $('#test' + i + 'chartdivaria').removeClass('d-none');
            $('#test' + i + 'chartdivspeed').removeClass('d-none');
        });

    }

    

   


    //NORP

    am4core.ready(function () {

        // Themes begin
        am4core.useTheme(am4themes_animated);
        // Themes end




        // Create chart instance
        var chart = am4core.create("chartdivaria", am4charts.XYChart);
        // Create axes
        var dateAxis = chart.xAxes.push(new am4charts.ValueAxis());
        var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());


        for (var i = 0; i < matrixH.length; i++) {
            createSeries("value" + i, "t = " + i, i);
        }

        // Create series
        function createSeries(s, name, index) {
            var series = chart.series.push(new am4charts.LineSeries());
            series.dataFields.valueY = "value" + s;
            series.dataFields.valueX = "valueX";
            series.name = name;

            var segment = series.segments.template;
            segment.interactionsEnabled = true;

            var hoverState = segment.states.create("hover");
            hoverState.properties.strokeWidth = 3;

            var dimmed = segment.states.create("dimmed");
            dimmed.properties.stroke = am4core.color("#dadada");

            segment.events.on("over", function (event) {
                processOver(event.target.parent.parent.parent);
            });

            segment.events.on("out", function (event) {
                processOut(event.target.parent.parent.parent);
            });

            var data = [];
            var value = 0;
            for (var i = 1; i < matrixH[index].length; i++) {
                value = matrixH[index][i];
                var dataItem = { valueX: i };
                dataItem["value" + s] = value;
                data.push(dataItem);
            }

            series.data = data;
            return series;
        }

        chart.legend = new am4charts.Legend();
        chart.legend.position = "right";
        chart.legend.scrollable = true;
        chart.legend.itemContainers.template.events.on("over", function (event) {
            processOver(event.target.dataItem.dataContext);
        })

        chart.legend.itemContainers.template.events.on("out", function (event) {
            processOut(event.target.dataItem.dataContext);
        })

        function processOver(hoveredSeries) {
            hoveredSeries.toFront();

            hoveredSeries.segments.each(function (segment) {
                segment.setState("hover");
            })

            chart.series.each(function (series) {
                if (series != hoveredSeries) {
                    series.segments.each(function (segment) {
                        segment.setState("dimmed");
                    })
                    series.bulletsContainer.setState("dimmed");
                }
            });
        }

        function processOut(hoveredSeries) {
            chart.series.each(function (series) {
                series.segments.each(function (segment) {
                    segment.setState("default");
                })
                series.bulletsContainer.setState("default");
            });
        }

    }); // end am4core.ready()

    am4core.ready(function () {

        // Themes begin
        am4core.useTheme(am4themes_animated);
        // Themes end




        // Create chart instance
        var chart = am4core.create("chartdivspeed", am4charts.XYChart);
        // Create axes
        var dateAxis = chart.xAxes.push(new am4charts.ValueAxis());
        var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());


        for (var i = 0; i < matrixU.length; i++) {
            createSeries("value" + i, "t = " + i, i);
        }

        // Create series
        function createSeries(s, name, index) {
            var series = chart.series.push(new am4charts.LineSeries());
            series.dataFields.valueY = "value" + s;
            series.dataFields.valueX = "valueX";
            series.name = name;

            var segment = series.segments.template;
            segment.interactionsEnabled = true;

            var hoverState = segment.states.create("hover");
            hoverState.properties.strokeWidth = 3;

            var dimmed = segment.states.create("dimmed");
            dimmed.properties.stroke = am4core.color("#dadada");

            segment.events.on("over", function (event) {
                processOver(event.target.parent.parent.parent);
            });

            segment.events.on("out", function (event) {
                processOut(event.target.parent.parent.parent);
            });

            var data = [];
            var value = 0;
            for (var i = 1; i < matrixU[index].length; i++) {
                value = matrixU[index][i];
                var dataItem = { valueX: i };
                dataItem["value" + s] = value;
                data.push(dataItem);
            }

            series.data = data;
            return series;
        }

        chart.legend = new am4charts.Legend();
        chart.legend.position = "right";
        chart.legend.scrollable = true;
        chart.legend.itemContainers.template.events.on("over", function (event) {
            processOver(event.target.dataItem.dataContext);
        })

        chart.legend.itemContainers.template.events.on("out", function (event) {
            processOut(event.target.dataItem.dataContext);
        })

        function processOver(hoveredSeries) {
            hoveredSeries.toFront();

            hoveredSeries.segments.each(function (segment) {
                segment.setState("hover");
            })

            chart.series.each(function (series) {
                if (series != hoveredSeries) {
                    series.segments.each(function (segment) {
                        segment.setState("dimmed");
                    })
                    series.bulletsContainer.setState("dimmed");
                }
            });
        }

        function processOut(hoveredSeries) {
            chart.series.each(function (series) {
                series.segments.each(function (segment) {
                    segment.setState("default");
                })
                series.bulletsContainer.setState("default");
            });
        }

    }); // end am4core.ready()

};






//////////MAP

var dojoConfig = { parseOnLoad: true };


dojo.require("dijit.dijit");
dojo.require("dijit.layout.BorderContainer");
dojo.require("dijit.layout.ContentPane");
dojo.require("dijit.form.CheckBox");
dojo.require("dijit.form.Button");
dojo.require("esri.map");
dojo.require("esri.tasks.DistanceParameters");
dojo.require("esri.tasks.query");
dojo.require("esri.tasks.locator");

var geometryService;
var baseLayers = [];
var map, locator;
var endGraphic;
var units;
var totalDistance = 0, inputPoints = [], legDistance = [];

function init() {
    esri.config.defaults.io.proxyUrl = "/proxy/";

    map = new esri.Map("map", {
        center: [25, 48.75],
        zoom: 5,
        slider: false
    });

    dojo.connect(map, "onClick", mapClickHandler);

    //add the basemaps (imagery, topo, street map). The visibility for the imagery and topo is set
    //to false for initial display
    var imagery = new esri.layers.ArcGISTiledMapServiceLayer("https://server.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer", {
        id: "imagery",
        visible: false
    });
    baseLayers.push(imagery);
    var streetMap = new esri.layers.ArcGISTiledMapServiceLayer("https://server.arcgisonline.com/ArcGIS/rest/services/World_Street_Map/MapServer", {
        id: "streetMap"
    });
    baseLayers.push(streetMap);
    var topoMap = new esri.layers.ArcGISTiledMapServiceLayer("https://server.arcgisonline.com/ArcGIS/rest/services/World_Topo_Map/MapServer", {
        id: "topo",
        visible: false
    });
    baseLayers.push(topoMap);
    map.addLayers(baseLayers);

    locator = new esri.tasks.Locator("https://sampleserver1.arcgisonline.com/ArcGIS/rest/services/Locators/ESRI_Geocode_USA/GeocodeServer");
    dojo.connect(locator, "onAddressToLocationsComplete", function (results) {
        if (results.length > 0) { //zoom to the first match
            var result = results[0];
            //results returned in geographic, convert to web mercator to display
            map.centerAndZoom(esri.geometry.geographicToWebMercator(result.location), 14);
        }
    });
    geometryService = new esri.tasks.GeometryService("https://utility.arcgisonline.com/ArcGIS/rest/services/Geometry/GeometryServer");
    dojo.connect(map, "onLoad", setPointsFromModel);
}

function setPointsFromModel() {
    var count = $('.js-points-count').data('count');
    for (var i = 0; i < count; i++) {
        var px = parseFloat($('#MapPoints_' + i + '__PointX').val());
        var py = parseFloat($('#MapPoints_' + i + '__PointY').val());
        var inPoint = new esri.geometry.Point(px, py, map.spatialReference);

        inputPoints.push(inPoint);
        setPoints(inPoint);
    }

}
function setPoints(mapPoint) {
    var markerSymbol = new esri.symbol.SimpleMarkerSymbol(esri.symbol.SimpleMarkerSymbol.STYLE_SQUARE, 12, new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_SOLID, new dojo.Color([204, 102, 51]), 1), new dojo.Color([158, 184, 71, 0.65]));
    var polylineSymbol = new esri.symbol.SimpleLineSymbol(esri.symbol.SimpleLineSymbol.STYLE_SOLID, new dojo.Color([204, 102, 51]), 4);
    var font = new esri.symbol.Font("18px", esri.symbol.Font.STYLE_NORMAL, esri.symbol.Font.VARIANT_NORMAL, esri.symbol.Font.WEIGHT_BOLDER);
    var textSymbol;

    if (inputPoints.length === 1) { //start location label
        textSymbol = new esri.symbol.TextSymbol("Start", font, new dojo.Color([204, 102, 51]));
        textSymbol.yoffset = 8;
        map.graphics.add(new esri.Graphic(mapPoint, textSymbol));
    }

    if (inputPoints.length >= 2) { //end location label
        textSymbol = new esri.symbol.TextSymbol("Finish", font, new dojo.Color([204, 102, 51]));
        textSymbol.yoffset = 8;
        if (endGraphic) { //if an end label already exists remove it
            map.graphics.remove(endGraphic);
        }
        endGraphic = new esri.Graphic(mapPoint, textSymbol);
        map.graphics.add(endGraphic);
    }

    //add a graphic for the clicked location
    map.graphics.add(new esri.Graphic(mapPoint, markerSymbol));

    //if there are two input points call the geometry service and perform the distance operation
    if (inputPoints.length >= 2) {
        dojo.style(dojo.byId("distanceDetails"), "display", "block");
        var distParams = new esri.tasks.DistanceParameters();
        distParams.distanceUnit = esri.tasks.GeometryService.UNIT_METER;

        distParams.geometry1 = inputPoints[inputPoints.length - 2];
        distParams.geometry2 = inputPoints[inputPoints.length - 1];
        distParams.geodesic = true;

        //draw a polyline to connect the input points
        var polyline = new esri.geometry.Polyline(map.spatialReference);
        polyline.addPath([distParams.geometry1, distParams.geometry2]);
        map.graphics.add(new esri.Graphic(polyline, polylineSymbol));
        //Calculate the geodesic distance
        geometryService.distance(distParams, function (distance) {
            if (isNaN(distance)) {
                distance = 0;
            }
            legDistance.push(distance);
            totalDistance += distance;
            var content = "";
            $('.js-line-lengthes').html("");
            for (var index = 0; index < legDistance.length; index++) {
                var leg = dojo.number.format(legDistance[index], {
                    places: 2
                }); 
                content = content + "<div class='mb-2' id='leg" + index + "'><b>Leg " + (index + 1) + "</b>" + ": " + leg
                    + "<button type='button' class='btn btn-success text-white btn-sm d-none ml-2' data-toggle='modal' data-target='#modalleg" + index + "'>show details</button></div>";

                $('.js-line-lengthes').append('<input type="text" class="form-control" ata-val="true" data-val-number="The field must be a number." data-val-required="The Double field is required."  id="Lengthes_' + index + '_" name="Lengthes[' + index + ']" value="' + legDistance[index] + '">')
                pieceLengths += legDistance[index] + ";";
            }
            content = content + "<b>Total:</b> " + dojo.number.format(totalDistance, {
                places: 2
            }) + " m <br />";
            $(".js-length-by-x").val(totalDistance);
            dojo.byId('distanceDetails').innerHTML = content;
        });
    }
}
function mapClickHandler(evt) {
    var inPoint = new esri.geometry.Point(evt.mapPoint.x, evt.mapPoint.y, map.spatialReference);

    inputPoints.push(inPoint);
    $('.js-points').html("");
    for (var i = 0; i < inputPoints.length; i++) {
        $('.js-points').append('<input type="text" class="form-control" data-val="true" id="MapPoints_' + i + '__PointX" name="MapPoints[' + i + '].PointX" value="' + inputPoints[i].x + '">' +
            '<input type="text" class= "form-control" data-val="true" id="MapPoints_' + i + '__PointY" name="MapPoints[' + i + '].PointY" value = "' + inputPoints[i].y + '" >' +
            '<input type="text" class="form-control" data-val="true" id="MapPoints_' + i + '__SequenceNumber" name="MapPoints[' + i + '].SequenceNumber" value="' + i + '">' +
            '<input type="text" class= "form-control" data-val="true" id="MapPoints_' + i + '__LatestWkid" name="MapPoints[' + i + '].LatestWkid" value = "' + inputPoints[i].spatialReference.latestWkid + '" >' +
            '<input type="text" class="form-control" data-val="true" id="MapPoints_' + i + '__Wkid" name="MapPoints[' + i + '].Wkid" value="' + inputPoints[i].spatialReference.wkid + '">');
    }
    $('.js-points-count').data('count', inputPoints.length);
    //define the symbology for the graphics
    setPoints(inPoint)
    //setPointsFromModel();
}

function resetMap() {
    map.graphics.clear();
    inputPoints.length = 0;
    totalDistance = 0;
    legDistance.length = 0;
    dojo.byId("distanceDetails").innerHTML = "";
    dojo.style(dojo.byId("distanceDetails"), "display", "none");
}

function locate() {
    resetMap();
    var address = {
        Zip: dojo.byId('zip').value
    };
    locator.addressToLocations(address);
}

function toggleBaseLayer(name) {
    if (map !== null) {
        dojo.forEach(baseLayers, function (baseLayer) {
            (baseLayer.id === name) ? baseLayer.show() : baseLayer.hide();
        });
    }
}

dojo.ready(init);