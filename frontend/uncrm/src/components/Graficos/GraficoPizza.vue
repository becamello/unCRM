<template>
  <highcharts :options="chartOptions"></highcharts>
</template>

<script>
import { Chart } from "highcharts-vue";

export default {
  name: "GraficoPizza",
  components: {
    highcharts: Chart,
  },
  props: {
    dados: Array,
    cores: Array,
    titulo: {
      type: String,
      required: true,
    },
    name: {
      type: String,
      default: "Títulos",
    },
    pointFormat: {
      type: String,
      default:
        "Gasto: <b>{point.gasto}</b> | Recebimento: <b>{point.recebimento}</b>",
    },
  },
  computed: {
    chartOptions() {
      return {
        chart: { type: "pie", backgroundColor: "transparent" },
        title: {
          text: this.titulo,
          style: {
            color: "var(--secondary)",
          },
        },
        credits: {
          enabled: false,
        },

        series: [
          {
            name: this.name,
            data: this.dados,
            colorByPoint: true,
            innerSize: "70%",
            dataLabels: {
              enabled: false,
              format: "{point.name} <br>{point.percentage:.1f}%",
              distance: 10,
              style: {
                fontSize: "12px",
                fontWeight: "normal",
                color: "var(--secondary)",
              },
            },
          },
        ],
        tooltip: {
          pointFormat: this.pointFormat,
        },
        legend: {
          enabled: true,
          align: "center",
          verticalAlign: "bottom",
          layout: "horizontal",
          itemStyle: {
            color: "var(--secondary)",
            fontWeight: "normal",
            cursor: "pointer",
          },
          itemHoverStyle: {
            color: "var(--text-hover)", 
          },
        },
        plotOptions: {
          pie: {
            allowPointSelect: true,
            cursor: "pointer",
            dataLabels: {
              enabled: false, 
            },
            showInLegend: true, 
          },
        },
      };
    },
  },
};
</script>
