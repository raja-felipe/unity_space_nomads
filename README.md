[![Review Assignment Due Date](https://classroom.github.com/assets/deadline-readme-button-24ddc0f5d75046c5622901739e7c5dd533143b0c8e959d652212380cedb1ea36.svg)](https://classroom.github.com/a/CibnTZFQ)

# Project 2 Report

Read the [project 2
specification](https://github.com/COMP30019/Project-2-Specification) for
details on what needs to be covered here. You may modify this template as you see fit, but please
keep the same general structure and headings.

Remember that you must also continue to maintain the Game Design Document (GDD)
in the `GDD.md` file (as discussed in the specification). We've provided a
placeholder for it [here](GDD.md).

## Table of Contents

* [Evaluation Plan](#evaluation-plan)
* [Evaluation Report](#evaluation-report)
* [Shaders and Special Effects](#shaders-and-special-effects)
* [Summary of Contributions](#summary-of-contributions)
* [References and External Resources](#references-and-external-resources)


## Evaluation Plan

### **Evaluation techniques**

**Think Aloud**

This technique will allow us to understand the real user's thoughts and
feelings, and identify any usability issues or confusing areas about the
game in real-time. Furthermore, this technique can also ensure
engagement during the game playing. We can obtain insights into their
decision-making process and observe any challenges they face.

Players who participate in the test will be asked to play the game and
try to verbalize their thoughts and feelings, these will also include
any thoughts about the decisions when they progress, and navigate the
game. There will be no help and instructions during the playing unless
they face some unconquerable difficulties. The whole testing process
will take 15 minutes about.

**Questionnaires**

This technique can offer the team many quantitative data that can be
easily analyzed. By using a structured way to gather feedback, this
technique can help the team identify common issues for game further
improvement.

For the participants who participate in the observation technique which
is think aloud. They will be asked to fill out this questionnaire after
thinking aloud but there is also an online recruitment for people who
are interested in the game.

### **Participants**

**Local recruitment:** The team will prepare the necessary devices to
test the game in libraries at the university. Asking people to
participate in the game evaluation.

**Online recruitment:** The team will also form a Google form for online
and local questionnaires which will be sent to Reddit and Discord
channels.

**Targeting:** participants who join this game evaluation should be in
the target age group which is 15-35 years old and at least have some
gaming experience. They can represent a mix of genders, and backgrounds
but for the game, they better have more interest in the FPS game.

### **Data Collection**

**Think aloud:** for this technique, the majority of data will be
collected by note-taking, and observation of player behaviour.
Participants will be asked to record their audio recordings during the
collection session.

**Questionnaires:** The majority of data will be quantitative data from
multiple choice questions and a few qualitative data from open-ended
questions. Participants will be asked to fill out digital questionnaires
by using a laptop after they finish the think aloud session. For
participants who join in online questionnaires, they can use any device
they have.

### **Timeline**

this evaluation will be conducted over two weeks. One week for data
collection and one week for data transcription, analysis, changes
implementation, and report writing.

### **Responsibilities**

**Evaluation manager:** Observe and give the necessary response to
participants. Ensure the timelines and tasks are successfully met.

**Data Collection member:** setting up equipment, note taking, and
collecting data during the session.

**Data analysis team:** This will be a task for all team members after
all evaluations are completed.

### **Data analysis**

The data will be transcribed, evaluating the severity in the table below



| Issue Description | Severity | Stractegy to address | Other notes |
| -------- | -------- | -------- | -------- |
| (example) enemies is overwhelming     | (example) medium     | (example) reduce the frequency of enemies spawn    | (example) 3 participants point out this problem    |

## Evaluation Report

TODO (due milestone 3) - see specification for details

## Shaders and Special Effects
### **Shader1: Disolve**
**Shader Link**: [Disolve.shader](https://github.com/COMP30019/project-2-synergy/blob/b471778edd2d88bfdd95a7d0e26a464e071cc1f1/Assets/Custom%20Shaders/Disolve.shader)
 **Shader Control script link**: [disolvecontrol](https://github.com/COMP30019/project-2-synergy/blob/b471778edd2d88bfdd95a7d0e26a464e071cc1f1/Assets/Scripts/disolveControl.cs)
### **Shader1 Demonstration**
<img src="Images/DemoShader1.gif" width="800">

This shader uses parts of the vertex and fragment shaders, mainly to achieve an inverse dissolve effect. The main inputs are _MainTex, _TextureForDisolve, _DisolveY and the control of the size _DisolveSize and the starting point _Starting, and the main output is the rendered colour. The main output is the rendered colour. In conjunction with the disolveControl C# script, a new material instance is created for each object and the _DisolveY parameter is incremented to achieve the reverse dissolve effect. This effect is mainly used in the in-game build function to allow a smooth emergence of newly built objects. Since an object may be built repeatedly, it is necessary to create a new material instance for each object.

TODO (due milestone 3) - see specification for details

## Summary of Contributions

TODO (due milestone 3) - see specification for details

## References and External Resources

TODO (to be continuously updated) - see specification for details
