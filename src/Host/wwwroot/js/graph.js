/// <reference path="../lib/jquery/jquery.js" />
/// <reference path="../../node_modules/plotly/index.js" />

/**
  @typedef {Object} stationReport
  @property {string} stationName
  @property {activityReport[]} activityReport
 */

/**
  @typedef {Object} activityReport
  @property {string} activity
  @property {MonthlyPerform[]} monthlyPerform
 */

/**
  @typedef {Object} MonthlyPerform
  @property {number} month
  @property {number} value
 */

var monthNames = [
  "January",
  "February",
  "March",
  "April",
  "May",
  "June",
  "July",
  "August",
  "September",
  "October",
  "November",
  "December"
];

var Colors = [
  { name: "aqua", value: "#00ffff" },
  { azure: "#f0ffff" },
  { beige: "#f5f5dc" },
  { black: "#000000" },
  { blue: "#0000ff" },
  { brown: "#a52a2a" },
  { cyan: "#00ffff" },
  { darkblue: "#00008b" },
  { darkcyan: "#008b8b" },
  { darkgrey: "#a9a9a9" },
  { darkgreen: "#006400" },
  { darkkhaki: "#bdb76b" },
  { darkmagenta: "#8b008b" },
  { darkolivegreen: "#556b2f" },
  { darkorange: "#ff8c00" },
  { darkorchid: "#9932cc" },
  { darkred: "#8b0000" },
  { darksalmon: "#e9967a" },
  { darkviolet: "#9400d3" },
  { fuchsia: "#ff00ff" },
  { gold: "#ffd700" },
  { green: "#008000" },
  { indigo: "#4b0082" },
  { khaki: "#f0e68c" },
  { lightblue: "#add8e6" },
  { lightcyan: "#e0ffff" },
  { lightgreen: "#90ee90" },
  { lightgrey: "#d3d3d3" },
  { lightpink: "#ffb6c1" },
  { lightyellow: "#ffffe0" },
  { lime: "#00ff00" },
  { magenta: "#ff00ff" },
  { maroon: "#800000" },
  { navy: "#000080" },
  { olive: "#808000" },
  { orange: "#ffa500" },
  { pink: "#ffc0cb" },
  { purple: "#800080" },
  { violet: "#800080" },
  { red: "#ff0000" },
  { silver: "#c0c0c0" },
  { white: "#ffffff" },
  { yellow: "#ffff00" }
];

$.ajax("/Company/GraphReport")
  .done(function(data) {
    /** @type {activityReport[]} */
    var models = data;
    var graphData = [];
    for (var i = 0; i < models.length; i++) {
      var model = models[i];
      var color = Colors.pop();
      graphData.push({
        x: model.monthlyPerform.map(i => monthNames[i.month]),
        y: model.monthlyPerform.map(i => i.value),
        type: "bar",
        name: model.activity,
        marker: {
          color: color.value,
          opacity: 0.7
        }
      });
    }

    var layout = {
      title: "Company Annual Report",
      xaxis: {
        tickangle: -45
      },
      barmode: "group"
    };
    const div = document.createElement("div");
    div.id = "newDiv" + i;
    const parentDiv = document.getElementById("myDiv");
    parentDiv.appendChild(div);
    Plotly.newPlot("newDiv" + i, graphData, layout);
  })
  .fail(function(err) {
    console.error(err);
  });
