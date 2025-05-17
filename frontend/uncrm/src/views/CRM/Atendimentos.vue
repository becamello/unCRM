<template>
    <v-overlay v-if="isLoading" absolute>
        <v-progress-circular indeterminate color="var(--primary-color)" size="30"></v-progress-circular>
    </v-overlay>
    <v-main class="pa-0" v-else>
        <v-container fluid>
            <v-row>
                <v-col cols="12" md="12" class="pa-6">
                    <v-row>
                        <v-col cols="12" md="7" sm="12" class="d-flex align-center justify-start">
                            <breadcrumbs />
                        </v-col>
                        <v-col cols="12" md="5" sm="12" class="d-flex align-center justify-end">
                            <div class="botoes">
                                <BotaoBase variante="primario" :iconeBtn="icons.adicionar">Novo atendimento</BotaoBase>
                                <BotaoBase variante="secundario" :iconeBtn="icons.filtro" @click="abrirFiltro">Filtros
                                </BotaoBase>
                            </div>
                        </v-col>

                    </v-row>
                    <tabela-geral :headers="headers" :items="atendimentos" :actions="acoes"
                        :height="400"></tabela-geral>
                </v-col>

                <v-navigation-drawer v-model="drawer" bottom fixed temporary right :width="360">
                    <v-list nav>
                        <v-list-item>
                            <v-list-item-content>
                                <v-list-item-title class="titulo-filtro">Filtro</v-list-item-title>
                            </v-list-item-content>
                        </v-list-item>
                    </v-list>

                    <v-list-item>
                        <v-list-item-content>
                            <v-form>
                                <div class="d-flex align-center mb-5">
                                    <p class="mb-0 mr-1">Próximo contato</p>
                                    <v-divider />
                                </div>
                                <DataPicker label="Data inicial" />
                                <DataPicker label="Data final" />
                                <v-autocomplete dense outlined deletable-chips multiple small-chips
                                    v-model="filtro.usuarios" :items="usuarios" item-value="id" item-text="login"
                                    label="Usuário" color="secondary" item-color="secondary" />
                                <div class="d-flex align-center mb-5">
                                    <p class="mb-0 mr-1">Cadastro</p>
                                    <v-divider />
                                </div>
                                <DataPicker label="Data inicial" />
                                <DataPicker label="Data final" />
                                <v-autocomplete dense outlined deletable-chips multiple small-chips
                                    v-model="filtro.usuarios" :items="usuarios" item-value="id" item-text="login"
                                    label="Usuário" color="secondary" item-color="secondary" />
                                <div class="d-flex align-center mb-5">
                                    <p class="mb-0 mr-1">Parecer</p>
                                    <v-divider />
                                </div>
                                <DataPicker label="Data inicial" />
                                <DataPicker label="Data final" />
                                <v-autocomplete dense outlined deletable-chips multiple small-chips
                                    v-model="filtro.usuarios" :items="usuarios" item-value="id" item-text="login"
                                    label="Usuário" color="secondary" item-color="secondary" />
                                <v-divider />
                            </v-form>
                        </v-list-item-content>
                    </v-list-item>

                    <template #append>
                        <v-list-item>
                            <v-list-item-content>
                                <v-row>
                                    <v-col cols="6" class="d-flex justify-center">
                                        <BotaoBase variante="secundario" width="100%">Limpar</BotaoBase>
                                    </v-col>
                                    <v-col cols="6" class="d-flex justify-center">
                                        <BotaoBase variante="primario" width="100%">Filtrar</BotaoBase>
                                    </v-col>
                                </v-row>
                            </v-list-item-content>
                        </v-list-item>
                    </template>
                </v-navigation-drawer>

            </v-row>
        </v-container>
    </v-main>
</template>

<script>
import TabelaGeral from "@/components/Tabela/TabelaGeral.vue";
import Breadcrumbs from "@/components/Breadcrumbs.vue";
import BotaoBase from "@/components/Base/BotaoBase.vue";
import DataPicker from "@/components/Base/DataPicker.vue";

import { icons } from "@/constants/icons";

import atendimentoService from "@/services/atendimento-service";
import { carregarTodosUsuarios } from '@/utils/usuarios';

import Atendimento from "@/models/Atendimento";

export default {
    name: "CRMAtendimentos",
    components: {
        TabelaGeral,
        Breadcrumbs,
        BotaoBase,
        DataPicker
    },
    data() {
        return {
            icons,
            headers: [
                { text: "Código", value: "id", width: "8%" },
                { text: "Tipo de atendimento", value: "tipoAtendimentoDescricao", width: "18%" },
                { text: "Pessoa", value: "pessoaNome", width: "34%" },
                { text: "Próximo contato", value: "proximoContato.descricao", width: "20%" },
                { text: "Cadastro", value: "dataUsuarioCadastro", width: "20%" },
                { text: "Status", value: "statusItem", align: "center" },
                { text: "Ações", value: "acoes", sortable: false },
            ],
            atendimentos: [],
            usuarios: [],
            acoes: [
                {
                    label: "Incluir parecer",
                    icon: icons.parecer,
                    handler: (item) => {
                        console.log("Editar", item);
                    },
                },
                {
                    label: "Histórico",
                    icon: icons.historico,
                    //   handler: (item) => {
                    //     console.log("Excluir", item);
                    //   },
                },
                {
                    label: "Alterar próximo contato",
                    icon: icons.proximoContato,
                    //   handler: (item) => {
                    //     console.log("Excluir", item);
                    //   },
                },
            ],
            drawer: false,
            isLoading: true,
            filtro: {
                usuarios: []
            }
        };
    },
    async mounted() {
        Promise.all([
            this.carregarAtendimentos(),
             this.usuarios = await carregarTodosUsuarios()
        ]).finally(() => this.isLoading = false)
    },
    methods: {
        async carregarAtendimentos() {
            try {
                const resultado = await atendimentoService.obterTodos();
                this.atendimentos = resultado.map((a) => new Atendimento(a));
            } catch (erro) {
                console.error("Erro ao carregar atendimentos:", erro);
            }
        },
        getNomeUsuario(id) {
            const usuario = this.usuarios.find(u => u.id === id);
            return usuario ? usuario.nome : "Usuário desconhecido";
        },
        formatarProximoContato(proximoContato) {
            if (!proximoContato) return "";
            const nome = this.getNomeUsuario(proximoContato.usuario);
            const dataFormatada = new Date(proximoContato.data).toLocaleDateString();
            return `${dataFormatada} - ${nome}`;
        },
        abrirFiltro() {
            this.drawer = true;
        }
    }
};
</script>

<style scoped>
.botoes {
    display: flex;
    flex-wrap: wrap;
    gap: 10px;
}

.titulo-filtro {
    color: var(--text-primary);
    font-size: 22px;
    font-weight: 500;
}
</style>