#pragma once

#include "../includes.h"

namespace extra
{

    void           glfwSetWindowCenter            (GLFWwindow *window);
    ImVec2         GetLocalCursor                 ();
    void           ShowStyleEditorWindow          (bool *child_sty);
    void           ShowColorExportWindow          (bool *child_colexp);
    bool           IsItemActiveAlt                (ImVec2 pos, int id);
    bool           GrabButton                     (ImVec2 pos, int random_int);
    void           HelpMarker                     (const char *desc);
    float          CenterHorizontal               ();
    void           TextCentered                   (std::string text, int type);

}