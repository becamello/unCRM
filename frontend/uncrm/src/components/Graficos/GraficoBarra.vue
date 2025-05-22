<template>
  <highcharts :options="chartOptions"></highcharts>
</template>

<script>
import { Chart } from "highcharts-vue";

export default {
  name: "GraficoBarra",
  components: {
    highcharts: Chart,
  },
  props: {
    categorias: Array,
    valores: Array,
    cores: Array,
    titulo: {
      type: String,
      default: "Atendimentos por Mês",
    },
  },
  computed: {
    chartOptions() {
      return {
        chart: {
          type: "column",
          backgroundColor: "transparent",
        },
        title: {
          text: this.titulo,
          style: {
            color: "var(--secondary)",
          },
        },
        credits: {
          enabled: false,
        },
        xAxis: {
          categories: this.categorias,
        },
        yAxis: {
          min: 0,
          title: {
            text: "Qtd. Atendimentos",
          },
          allowDecimals: false,
        },
        series: [
          {
            name: "Atendimentos",
            data: this.valores,
          },
        ],
        legend: {
           enabled: false,
        },
        tooltip: {
          formatter: function () {
            const mesesNomesCompletos = [
              "Janeiro",
              "Fevereiro",
              "Março",
              "Abril",
              "Maio",
              "Junho",
              "Julho",
              "Agosto",
              "Setembro",
              "Outubro",
              "Novembro",
              "Dezembro",
            ];
            const mesIndex = this.point.index;
            const mesNomeCompleto =
              mesesNomesCompletos[mesIndex] || "Mês desconhecido";

            return `${mesNomeCompleto}: <b>${this.y}</b> atendimentos`;
          },
        },
      };
    },
  },
};
</script>