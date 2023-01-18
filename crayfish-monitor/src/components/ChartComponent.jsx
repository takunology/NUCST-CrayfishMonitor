import React from "react";
import ReactECharts from "echarts-for-react"

let data_x = new Array();
let data_y = new Array();

for(let i = 0; i < 1000; i++) {
  data_x.push(i);
  data_y.push(Math.random() * 100);
}

function ChartComponent() {
  const option = {
    textStyle: {
      fontSize: 14
    },
    xAxis: {
      type: 'category',
      data: data_x,
      name: 'Time'
    },
    yAxis: {
      type: 'value',
      name: 'Voltage [V]'
    },
    series: [{
        data: data_y,
        type: 'line',
        name: '個体A',
        symbol: 'none',
        sampling: 'lttb',
      }
    ],
    toolbox: {
      feature: {
        dataZoom: {
          yAxisIndex: 'none'
        },
        restore: {show: false},
        saveAsImage: {type: 'png'}
      }
    },
    dataZoom: [
      {
          id: 'dataZoomX',
          type: 'slider',
          xAxisIndex: [0],
          filterMode: 'filter'
      },
      {
          id: 'dataZoomY',
          type: 'inside',
          yAxisIndex: [0],
          filterMode: 'empty'
      }
    ],
    tooltip: {
      trigger: 'axis',
      position: function (pt) {
        return [pt[0], '10%'];
      }
    },
  };
  return <ReactECharts option={option}/>
}

export default ChartComponent;