/// <reference path="../lib/jquery/jquery.js" />
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
  {name: "azure", value: "#f0ffff" },
  { name:" beige", value: "#f5f5dc" },
  { name:"black", value: "#000000" },
  { name:"blue", value: "#0000ff" },
  { name: "brown", value: "#a52a2a" },
  { name:"cyan", value: "#00ffff" },
  { name:"darkblue", value: "#00008b" },
  { name: "darkcyan", value: "#008b8b" },
  { name:"darkgrey", value: "#a9a9a9" },
  { name:"darkgreen", value: "#006400" },
  { name:"darkkhaki", value: "#bdb76b" },
  { name:"darkmagenta", value: "#8b008b" },
  { name:"darkolivegreen" , value: "#556b2f" },
  { name:"darkorange", value: "#ff8c00" },
  { name: "darkorchid", value: "#9932cc" },
  { name:"darkred", value: "#8b0000" },
  { name:"darksalmon" ,value: "#e9967a" },
  { name:"darkviolet", value: "#9400d3" },
  { name:"fuchsia", value: "#ff00ff" },
  { name:"gold", value: "#ffd700" },
  { name:"green", value:"#008000" },
  { name:"indigo", value: "#4b0082" },
  { name: "khaki", value:"#f0e68c" },
  { name:"lightblue", value: "#add8e6" },
  { name:"lightcyan" , value:"#e0ffff" },
  { name:"lightgreen", value: "#90ee90" },
  { name:"lightgrey", value: "#d3d3d3" },
  { name:"lightpink", value: "#ffb6c1" },
  { name:"lightyellow", value: "#ffffe0" },
  { name:"lime", value: "#00ff00" },
  { name: "magenta", value: "#ff00ff" },
  { name: "maroon", value: "#800000" },
  { name:"navy", value: "#000080" },
  { name:" olive", value: "#808000" },
  { name:"orange", value: "#ffa500" },
  { name: "pink", value: "#ffc0cb" },
  { name: "purple" ,value: "#800080" },
  { name: "violet", value: "#800080" },
  { name:" red", value: "#ff0000" },
  { name:" silver" , value: "#c0c0c0" },
  { name:" white" ,value: "#ffffff" },
  { name:" yellow" , value: "#ffff00" }
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
          showlegend: false,

      
    
      xaxis: {
          tickangle: -45
      },
      barmode: "group"
      };

      
    const div = document.createElement("div");
    div.id = "newDiv" + i;
    const parentDiv = document.getElementById("myDiv");
      parentDiv.appendChild(div);
      Plotly.newPlot("newDiv" + i, graphData, layout, { editable: false });
  })
  .fail(function(err) {
    console.error(err);
  });
