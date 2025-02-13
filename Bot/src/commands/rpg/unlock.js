const { SlashCommandBuilder, EmbedBuilder, StringSelectMenuBuilder, StringSelectMenuOptionBuilder, ActionRowBuilder } = require('discord.js');
const apiService = require('../../services/apiService');

module.exports = {
    data: new SlashCommandBuilder()
        .setName('unlock')
        .setDescription('Débloque une compétence pour votre personnage'),

    async execute(interaction) {
        try {
            await interaction.deferReply();
            const discordId = interaction.user.id;

            const player = await apiService.get(`/player/${discordId}`);
            
            if (!player.characters || player.characters.length === 0) {
                return await interaction.editReply('Vous n\'avez aucun personnage.');
            }

            // Sélection du personnage
            const characterSelect = new StringSelectMenuBuilder()
                .setCustomId('character-select')
                .setPlaceholder('Sélectionnez un personnage')
                .addOptions(
                    player.characters.map(char => 
                        new StringSelectMenuOptionBuilder()
                            .setLabel(char.name)
                            .setDescription(`${char.race} ${char.classe} niveau ${char.niveau}`)
                            .setValue(char.id.toString())
                    )
                );

            const characterRow = new ActionRowBuilder().addComponents(characterSelect);
            const response = await interaction.editReply({
                content: 'Choisissez un personnage :',
                components: [characterRow]
            });

            try {
                const characterSelection = await response.awaitMessageComponent({
                    filter: i => i.user.id === interaction.user.id,
                    time: 30000
                });

                const character = await apiService.get(`/character/${characterSelection.values[0]}`);
                const skillTree = await apiService.get(`/character/${characterSelection.values[0]}/skilltree`);

                // Filtrer les compétences débloquables
                const unlockedSkills = character.competences.filter(c => c.estDebloque).map(c => c.id);
                const availableSkills = skillTree.nodes.filter(node => 
                    !unlockedSkills.includes(node.id) && 
                    node.prerequisSkillIds.every(preReqId => unlockedSkills.includes(preReqId))
                );

                if (availableSkills.length === 0) {
                    return await characterSelection.update({
                        content: 'Aucune compétence disponible à débloquer pour le moment.',
                        components: []
                    });
                }

                // Sélection de la compétence
                const skillSelect = new StringSelectMenuBuilder()
                    .setCustomId('skill-select')
                    .setPlaceholder('Sélectionnez une compétence à débloquer')
                    .addOptions(
                        availableSkills.map(skill => 
                            new StringSelectMenuOptionBuilder()
                                .setLabel(skill.nom)
                                .setDescription(`Coût: ${skill.coutPoints} points`)
                                .setValue(skill.id.toString())
                        )
                    );

                const skillRow = new ActionRowBuilder().addComponents(skillSelect);
                await characterSelection.update({
                    content: `Choisissez une compétence à débloquer (Points disponibles: ${character.pointsDeCompetences})`,
                    components: [skillRow]
                });

                const skillSelection = await response.awaitMessageComponent({
                    filter: i => i.user.id === interaction.user.id,
                    time: 30000
                });

                const result = await apiService.post(
                    `/character/${characterSelection.values[0]}/competence?skillId=${skillSelection.values[0]}`
                );

                const embed = new EmbedBuilder()
                    .setColor('#00ff00')
                    .setTitle('✨ Compétence débloquée !')
                    .setDescription(`La compétence a été débloquée avec succès.\nPoints de compétences restants: ${result.pointsDeCompetences}`);

                await skillSelection.update({ embeds: [embed], components: [] });
            } catch (e) {
                await interaction.editReply({
                    content: 'Temps écoulé ou erreur lors de la sélection.',
                    components: []
                });
            }
        } catch (error) {
            console.error('Erreur lors du déblocage de la compétence:', error);
            await interaction.editReply('Une erreur est survenue lors du déblocage de la compétence.');
        }
    },
}; 