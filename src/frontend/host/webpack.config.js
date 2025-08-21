require("dotenv").config({
  path: `.env.${process.env.NODE_ENV || "development"}`,
});

const { merge } = require("webpack-merge");
const singleSpaDefaults = require("webpack-config-single-spa-ts");
const HtmlWebpackPlugin = require("html-webpack-plugin");

module.exports = (webpackConfigEnv, argv) => {
  const orgName = "resume-site-front";
  const defaultConfig = singleSpaDefaults({
    orgName,
    projectName: "root-config",
    webpackConfigEnv,
    argv,
    disableHtmlGeneration: true,
  });

  console.log(process.env.ROOT_FRONT_URL);

  var debugConfig = {
    mode: "development",
    devtool: "source-map",
    resolve: { extensions: ["ts", "js"] },
  };

  return merge(
    defaultConfig,
    {
      // modify the webpack config however you'd like to by adding to this object
      plugins: [
        new HtmlWebpackPlugin({
          inject: false,
          template: "src/index.ejs",
          templateParameters: {
            isLocal: webpackConfigEnv && webpackConfigEnv.isLocal,
            REACT_URL: process.env.REACT_FRONT_URL,
            VUE_URL: process.env.VUE_FRONT_URL,
            ROOT_URL: process.env.ROOT_FRONT_URL,
            orgName,
          },
        }),
      ],
    },
    debugConfig
  );
};
