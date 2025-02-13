const { SlashCommandBuilder } = require('@discordjs/builders');
const { EmbedBuilder } = require('discord.js');
const apiService = require('../../services/apiService');

module.exports = {
    data: new SlashCommandBuilder()
        .setName('create')
        .setDescription('Créer un nouveau personnage')
        .addStringOption(option =>
            option.setName('name')
                .setDescription('Nom du personnage')
                .setRequired(true))
        .addStringOption(option =>
            option.setName('race')
                .setDescription('Race du personnage')
                .setRequired(true)
                .addChoices(
                    { name: 'Humain', value: 'Humain' },
                    { name: 'Elfe', value: 'Elfe' },
                    { name: 'Nain', value: 'Nain' },
                    { name: 'Orc', value: 'Orc' }
                ))
        .addStringOption(option =>
            option.setName('classe')
                .setDescription('Classe du personnage')
                .setRequired(true)
                .addChoices(
                    { name: 'Guerrier', value: 'Guerrier' },
                    { name: 'Mage', value: 'Mage' },
                    { name: 'Voleur', value: 'Voleur' },
                    { name: 'Prêtre', value: 'Pretre' }
                )),

    async execute(interaction) {
        try {
            await interaction.deferReply();

            const name = interaction.options.getString('name');
            const race = interaction.options.getString('race');
            const classe = interaction.options.getString('classe');
            const discordId = interaction.user.id;

            // Création du personnage via l'API
            const character = await apiService.post(`/player/${discordId}/character`, {
                name,
                race,
                classe
            });

            const embed = new EmbedBuilder()
                .setTitle('🎉 Personnage créé avec succès !')
                .setColor('#00ff00')
                .addFields(
                    { name: 'Nom', value: character.name, inline: true },
                    { name: 'Race', value: character.race, inline: true },
                    { name: 'Classe', value: character.classe, inline: true },
                    { name: 'Niveau', value: character.niveau.toString(), inline: true },
                    { name: 'PV', value: `${character.pv}/${character.pvMax}`, inline: true },
                    { name: 'Mana', value: `${character.mana}/${character.manaMax}`, inline: true }
                );

            await interaction.editReply({ embeds: [embed] });
        } catch (error) {
            console.error('Erreur lors de la création du personnage:', error);
            await interaction.editReply('Une erreur est survenue lors de la création du personnage.');
        }
    },
}; 