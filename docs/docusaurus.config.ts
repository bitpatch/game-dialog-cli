import {themes as prismThemes} from 'prism-react-renderer';
import type {Config} from '@docusaurus/types';
import type * as Preset from '@docusaurus/preset-classic';

// This runs in Node.js - Don't use client-side code here (browser APIs, JSX...)

const config: Config = {
  title: 'Game Dialog Script',
  tagline: 'A simple language for writing game dialogs with integrated logic',
  favicon: 'img/favicon.ico',

  // Future flags, see https://docusaurus.io/docs/api/docusaurus-config#future
  future: {
    v4: true, // Improve compatibility with the upcoming Docusaurus v4
  },

  // Set the production url of your site here
  url: 'https://bitpatch.github.io',
  // Set the /<baseUrl>/ pathname under which your site is served
  // For GitHub pages deployment, it is often '/<projectName>/'
  baseUrl: '/game-dialog/',

  // GitHub pages deployment config.
  organizationName: 'bitpatch',
  projectName: 'game-dialog',

  onBrokenLinks: 'throw',

  // Even if you don't use internationalization, you can use this field to set
  // useful metadata like html lang. For example, if your site is Chinese, you
  // may want to replace "en" with "zh-Hans".
  i18n: {
    defaultLocale: 'en',
    locales: ['en'],
  },

  presets: [
    [
      'classic',
      {
        docs: {
          routeBasePath: '/', // Docs as the homepage
          sidebarPath: './sidebars.ts',
          editUrl:
            'https://github.com/bitpatch/game-dialog/tree/main/docs/',
        },
        blog: false, // Disabled for now
        theme: {
          customCss: './src/css/custom.css',
        },
      } satisfies Preset.Options,
    ],
  ],

  themeConfig: {
    // Replace with your project's social card
    image: 'img/docusaurus-social-card.jpg',
    colorMode: {
      respectPrefersColorScheme: true,
    },
    navbar: {
      title: 'Game Dialog Script',
      logo: {
        alt: 'Game Dialog Script Logo',
        src: 'img/logo.svg',
      },
      items: [
        {
          href: 'https://github.com/bitpatch/game-dialog',
          label: 'GitHub',
          position: 'right',
        },
      ],
    },
    footer: {
      style: 'dark',
      links: [
        {
          title: 'Docs',
          items: [
            {
              label: 'Getting Started',
              to: '/',
            },
            {
              label: 'Installation',
              to: '/installation',
            },
          ],
        },
        {
          title: 'More',
          items: [
            {
              label: 'GitHub',
              href: 'https://github.com/bitpatch/game-dialog',
            },
            {
              label: 'NuGet',
              href: 'https://www.nuget.org/packages/gdialog/',
            },
          ],
        },
      ],
      copyright: `Copyright © ${new Date().getFullYear()} BitPatch. Built with Docusaurus.`,
    },
    prism: {
      theme: prismThemes.github,
      darkTheme: prismThemes.dracula,
      additionalLanguages: ['csharp', 'bash'],
    },
  } satisfies Preset.ThemeConfig,
};

export default config;
