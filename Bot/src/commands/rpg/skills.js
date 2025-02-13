const { SlashCommandBuilder, EmbedBuilder, StringSelectMenuBuilder, StringSelectMenuOptionBuilder, ActionRowBuilder } = require('discord.js');
const apiService = require('../../services/apiService');

function createSkillEmbed(skill, character, isUnlocked, canUnlock, allSkills) {
    const status = isUnlocked ? '🟢' : (canUnlock ? '🟡' : '🔴');
    const title = `${status} ${skill.nom} (ID: ${skill.id})`;
    
    // Obtenir les noms des compétences prérequises
    const prerequisNames = skill.prerequisSkillIds.map(preReqId => {
        const preReqSkill = allSkills.find(s => s.id === preReqId);
        const preReqStatus = character.competences.some(c => c.id === preReqId && c.estDebloque) ? '🟢' : '🔴';
        return `${preReqStatus} ${preReqSkill ? preReqSkill.nom : `Compétence ${preReqId}`}`;
    });

    return {
        name: title,
        value: [
            `> ${skill.description}`,
            `**Coût:** ${skill.coutPoints} points`,
            skill.effets.length > 0 ? '**Effets:**' : '',
            ...skill.effets.map(effet => `• ${effet.effectName}: ${effet.value}`),
            prerequisNames.length > 0 ? `\n**Prérequis:**\n${prerequisNames.map(name => `▫️ ${name}`).join('\n')}` : ''
        ].filter(Boolean).join('\n'),
        inline: false
    };
}

module.exports = {
    data: new SlashCommandBuilder()
        .setName('skills')
        .setDescription('Affiche l\'arbre de compétences de votre personnage'),

    async execute(interaction) {
        try {
            await interaction.deferReply();
            const discordId = interaction.user.id;

            const player = await apiService.get(`/player/${discordId}`);
            
            if (!player.characters || player.characters.length === 0) {
                return await interaction.editReply('Vous n\'avez aucun personnage. Utilisez /create pour en créer un !');
            }

            const select = new StringSelectMenuBuilder()
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

            const row = new ActionRowBuilder().addComponents(select);
            const response = await interaction.editReply({
                content: 'Choisissez un personnage pour voir ses compétences :',
                components: [row]
            });

            try {
                const selection = await response.awaitMessageComponent({
                    filter: i => i.user.id === interaction.user.id,
                    time: 30000
                });

                const character = await apiService.get(`/character/${selection.values[0]}`);
                const skillTree = await apiService.get(`/character/${selection.values[0]}/skilltree`);

                // Grouper les compétences par niveau
                const skillsByLevel = {};
                skillTree.nodes.forEach(node => {
                    const level = node.prerequisSkillIds.length;
                    if (!skillsByLevel[level]) skillsByLevel[level] = [];
                    skillsByLevel[level].push(node);
                });

                // Créer un embed pour chaque niveau
                const embeds = Object.entries(skillsByLevel).map(([level, skills]) => {
                    const embed = new EmbedBuilder()
                        .setColor(level === '0' ? '#4CAF50' : '#2196F3')
                        .setTitle(level === '0' ? '📚 Compétences de base' : `📖 Niveau ${level} - Requiert ${level} compétence${level > 1 ? 's' : ''}`);

                    skills.forEach(skill => {
                        const isUnlocked = character.competences.some(c => c.id === skill.id && c.estDebloque);
                        const canUnlock = skill.prerequisSkillIds.every(preReqId => 
                            character.competences.some(c => c.id === preReqId && c.estDebloque)
                        );

                        embed.addFields(createSkillEmbed(skill, character, isUnlocked, canUnlock, skillTree.nodes));
                    });

                    return embed;
                });

                // Ajouter l'embed principal
                const mainEmbed = new EmbedBuilder()
                    .setColor('#0099ff')
                    .setTitle(`🌳 Arbre de compétences de ${character.name}`)
                    .setDescription([
                        `**${character.race} ${character.classe}**`,
                        `Points de compétences disponibles: **${character.pointsDeCompetences}**`,
                        '',
                        '**Légende:**',
                        '🟢 Débloqué',
                        '🟡 Disponible',
                        '🔴 Verrouillé'
                    ].join('\n'));

                await selection.update({ 
                    embeds: [mainEmbed, ...embeds],
                    components: [] 
                });

            } catch (e) {
                console.error('Erreur lors de la sélection:', e);
                await interaction.editReply({
                    content: 'Le temps de sélection est écoulé ou une erreur est survenue.',
                    components: []
                });
            }
        } catch (error) {
            console.error('Erreur:', error);
            await interaction.editReply('Une erreur est survenue.');
        }
    },
}; 