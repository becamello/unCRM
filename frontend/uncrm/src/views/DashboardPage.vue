<template>
  <v-main class="pa-0">
    <v-container fluid>
      <v-row>
        <v-col v-col cols="12" md="12" class="pa-6">
          <v-row>
            <v-col
              cols="12"
              md="7"
              sm="12"
              class="d-flex align-center justify-start"
            >
              <Breadcrumbs />
            </v-col>
          </v-row>
        </v-col>
      </v-row>
      <v-row>
        <v-col cols="12" md="6" sm="12">
          <v-card class="pa-5 cardGraficos">
            <GraficoPizza
              :dados="dadosGraficoPizzaPessoa"
              :cores="cores"
              titulo="Atendimentos por Pessoa"
              name="Atendimentos"
              pointFormat="Total de atendimentos: <b>{point.y}</b>"
            />
          </v-card>
        </v-col>
        <v-col cols="12" md="6" sm="12">
          <v-card class="pa-5 cardGraficos">
            <GraficoPizza
              :dados="dadosGraficoPizzaUsuario"
              :cores="cores"
              titulo="Atendimentos por Usuário"
              name="Atendimentos"
              pointFormat="Total de atendimentos: <b>{point.y}</b>"
            />
          </v-card>
        </v-col>
      </v-row>
      <v-row>
        <v-col cols="12" md="12" sm="12">
          <v-card class="pa-5 cardGraficos">
            <GraficoBarra
              :categorias="categoriasMeses"
              :valores="dadosGraficoBarraMes"
              :cores="cores"
              titulo="Atendimentos por Mês (2025)"
            />
          </v-card>
        </v-col>
      </v-row>
    </v-container>
  </v-main>
</template>
<script>
import Breadcrumbs from "@/components/Breadcrumbs.vue";
import GraficoPizza from "@/components/Graficos/GraficoPizza.vue";
import GraficoBarra from "@/components/Graficos/GraficoBarra.vue";
import { carregarAtendimentos } from "@/utils/atendimentos";
import Atendimento from "@/models/Atendimento";

export default {
  components: {
    GraficoPizza,
    Breadcrumbs,
    GraficoBarra,
  },
  props: {
    cores: Array,
  },
  data() {
    return {
      dadosGraficoPizzaPessoa: [],
      dadosGraficoPizzaUsuario: [],
      dadosGraficoBarraMes: [],
      categoriasMeses: [],
    };
  },
  mounted() {
    this.fetchData();
  },
  methods: {
    async fetchData() {
      try {
        const response = await carregarAtendimentos({});
        const atendimentos = response.map((item) => new Atendimento(item));

        const atendimentosPorPessoa = {};
        atendimentos.forEach((a) => {
          const nomePessoa = a.pessoaNome || "Sem Nome";
          atendimentosPorPessoa[nomePessoa] =
            (atendimentosPorPessoa[nomePessoa] || 0) + 1;
        });
        this.dadosGraficoPizzaPessoa = Object.entries(
          atendimentosPorPessoa
        ).map(([nome, count]) => ({ name: nome, y: count }));

        const atendimentosPorUsuario = {};
        atendimentos.forEach((a) => {
          const login = a.usuarioCriadorLogin || "Sem Usuário";
          atendimentosPorUsuario[login] =
            (atendimentosPorUsuario[login] || 0) + 1;
        });
        this.dadosGraficoPizzaUsuario = Object.entries(
          atendimentosPorUsuario
        ).map(([login, count]) => ({ name: login, y: count }));

        const meses = Array(12).fill(0);
        atendimentos.forEach((a) => {
          const data = new Date(a.dataCadastro);
          if (data.getFullYear() === 2025) {
            const mes = data.getMonth(); 
            meses[mes]++;
          }
        });

        this.dadosGraficoBarraMes = meses;
        this.categoriasMeses = [
          "Jan",
          "Fev",
          "Mar",
          "Abr",
          "Mai",
          "Jun",
          "Jul",
          "Ago",
          "Set",
          "Out",
          "Nov",
          "Dez",
        ];
      } catch (error) {
        console.error("Erro ao buscar atendimentos:", error);
      }
    },
  },
};
</script>

<style scoped>
.cardGraficos {
  border-radius: 20px;
}
</style>